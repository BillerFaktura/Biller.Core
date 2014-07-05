using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Update
{
    /// <summary>
    /// From https://github.com/leetreveil/.NET-Auto-Update
    /// </summary>
    public class Progress
    {
        public long Current { get; set; }
        public long Total { get; set; }
    }
}
