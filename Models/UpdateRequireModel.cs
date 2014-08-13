using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Models
{
    public class UpdateRequireModel
    {
        public double Version { get; set; }
        public string Name { get; set; }
        public string RequiredGuID { get; set; }
    }
}
