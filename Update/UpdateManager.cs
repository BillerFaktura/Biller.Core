using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Biller.Core.Update
{
    /// <summary>
    /// Plugins should register to this Manager when they get initalized
    /// </summary>
    public class UpdateManager : Utils.PropertyChangedHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public UpdateManager()
        {
            UpdateSources = new List<string>();
            RegisteredApps = new List<Models.AppModel>();
            CollectedApps = new List<Models.UpdateModel>();
        }

        public void Register(Models.AppModel app)
        {
            if (!UpdateSources.Contains(app.UpdateSource))
                UpdateSources.Add(app.UpdateSource);
            RegisteredApps.Add(app);
        }

        public void CheckForUpdates()
        {
            GetSources();
        }

        public event EventHandler CheckForUpdatesCompleted;

        public ObservableCollection<Models.UpdateModel> UpdateableApps { get { return GetValue(() => UpdateableApps); } set { SetValue(value); }}

        public ObservableCollection<Models.UpdateModel> NonUpdateableApps { get { return GetValue(() => NonUpdateableApps); } set { SetValue(value); } }

        private List<string> UpdateSources { get { return GetValue(() => UpdateSources); } set { SetValue(value); } }

        private List<Models.AppModel> RegisteredApps { get; set; }

        private List<Models.UpdateModel> CollectedApps { get; set; }

        private int CompletedRequests;

        private void GetSources()
        {
            CompletedRequests = 0;
            foreach (var source in UpdateSources)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(source));
                request.Proxy = null;
                DoWithResponse(request, (response) =>
                {
                    CompletedRequests += 1;
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var entries = JsonConvert.DeserializeObject<Models.UpdateModels>(body);
                    foreach (var entry in entries)
                        if (!CollectedApps.Contains(entry))
                            CollectedApps.Add(entry);
                    if (CompletedRequests == UpdateSources.Count())
                        CompareVersions();
                }, (response) =>
                {
                    CompletedRequests += 1;
                    if (CompletedRequests == UpdateSources.Count())
                        CompareVersions();
                });
            }
        }

        void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction, Action<object> requestFailedAction)
        {
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    try
                    {
                        var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                        responseAction(response);
                    }
                    catch(Exception e)
                    {
                        logger.ErrorException("Failed getting update URL", e);
                        requestFailedAction(null);
                    }
                    
                }), request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        private void CompareVersions()
        {
            UpdateableApps = new ObservableCollection<Models.UpdateModel>();
            NonUpdateableApps = new ObservableCollection<Models.UpdateModel>();

            foreach (var app in RegisteredApps)
            {
                var updateApp = (from updates in CollectedApps where updates.GuID == app.GuID select updates).FirstOrDefault();
                if (updateApp == null)
                    continue;

                if (updateApp.Version > app.Version)
                {
                    try
                    {
                        // Check dependencies
                        var missingDependency = new List<Models.UpdateRequireModel>();
                        foreach (var dep in updateApp.RequiredApps)
                        {
                            var length = (from dependency in RegisteredApps where dependency.GuID == dep.RequiredGuID select dependency).Count();
                            if (length == 0)
                                missingDependency.Add(dep);
                        }
                        if (missingDependency.Count == 0)
                            UpdateableApps.Add(updateApp);
                        else
                            NonUpdateableApps.Add(updateApp);
                    }
                    catch(Exception e)
                    {
                        logger.ErrorException("Error while comparing version for app " + app.Title, e);
                        NonUpdateableApps.Add(updateApp);
                    }
                }
            }
            EventHandler handler = CheckForUpdatesCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Update(Models.UpdateModel app)
        {

        }
    }
}
