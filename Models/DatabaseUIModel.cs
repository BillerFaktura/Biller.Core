using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Biller.Core.Models
{
    public class DatabaseUIModel
    {
        public DatabaseUIModel()
        { }

        public DatabaseUIModel(Interfaces.IDatabase database, Control settings)
        {
            Settings = settings;
            Database = database;
        }

        public Control Settings { get; set; }

        public Interfaces.IDatabase Database { get; set; }
    }
}
