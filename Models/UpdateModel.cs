using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Models
{
    public class UpdateModel
    {
        public UpdateModel()
        {
            RequiredApps = new ObservableCollection<UpdateRequireModel>();
            Title = String.Empty;
            Description = String.Empty;
            GuID = String.Empty;
            UpdateURL = String.Empty;
            ChangelogURL = String.Empty;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GuID { get; set; }
        public string UpdateURL { get; set; }
        public double Version { get; set; }
        public string ChangelogURL { get; set; }
        public ObservableCollection<UpdateRequireModel> RequiredApps { get; set; }
    }

    public class UpdateModels : ObservableCollection<UpdateModel> { }
}
