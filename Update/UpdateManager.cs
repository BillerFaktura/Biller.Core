using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Biller.Core.Update
{
    /// <summary>
    /// Plugins should register to this Manager when they get initalized
    /// </summary>
    public class UpdateManager : Utils.PropertyChangedHelper
    {
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

        public ObservableCollection<Models.UpdateModel> UpdateableApps { get { return GetValue(() => UpdateableApps); } set { SetValue(value); }}

        public ObservableCollection<Models.UpdateModel> NonUpdateableApps { get { return GetValue(() => NonUpdateableApps); } set { SetValue(value); } }

        private List<string> UpdateSources { get { return GetValue(() => UpdateSources); } set { SetValue(value); } }

        private List<Models.AppModel> RegisteredApps { get; set; }

        private List<Models.UpdateModel> CollectedApps { get; set; }

        private void GetSources()
        {
            foreach (var source in UpdateSources)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(source));
                request.Proxy = null;
                DoWithResponse(request, (response) =>
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var entries = JsonConvert.DeserializeObject<Models.UpdateModels>(body);
                    foreach (var entry in entries)
                        if (!CollectedApps.Contains(entry))
                            CollectedApps.Add(entry);
                    CompareVersions();
                });
            }
        }

        void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
        {
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                    responseAction(response);
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
                if (String.IsNullOrEmpty(updateApp.GuID))
                    continue;

                if (updateApp.Version > app.Version)
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
            }
        }
    }
}
