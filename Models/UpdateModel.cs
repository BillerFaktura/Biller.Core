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
        public string Title { get; set; }
        public string Description { get; set; }
        public string UpdateURL { get; set; }
        public double Version { get; set; }
        public List<UpdateRequireModel> RequiredApps { get; set; }
    }

    public class UpdateModels : ObservableCollection<UpdateModel> { }
}
