using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Models
{
    public class AppModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string GuID { get; set; }
        public string UpdateSource { get; set; }
        public double Version { get; set; }
    }
}
