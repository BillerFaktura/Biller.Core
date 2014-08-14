using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace Biller.Core.Database
{
    public class AppSettings : INotifyPropertyChanged
    {
        public AppSettings()
        { }

        public void Load()
        {
            try
            {
                if (!File.Exists("appsettings.json"))
                    using (StreamWriter writer = File.CreateText("appsettings.json"))
                        writer.Write(JsonConvert.SerializeObject(this));
            }
            catch(Exception e)
            {
                
            }
            

            using (StreamReader reader = File.OpenText("appsettings.json"))
            {
                var settings = JsonConvert.DeserializeObject<AppSettings>(reader.ReadToEnd());
                Database = settings.Database;
                DatabaseOptions = settings.DatabaseOptions;
            }
        }

        public void Save()
        {
            using (StreamWriter writer = File.CreateText("appsettings.json"))
                writer.Write(JsonConvert.SerializeObject(this));
        }

        private string database;
        public string Database
        {
            get { return database; }

            set
            {
                if (database == value) return;
                database = value;
                OnPropertyChanged("Database");
            }
        }

        private string databaseOptions;
        public string DatabaseOptions
        {
            get { return databaseOptions; }

            set
            {
                if (databaseOptions == value) return;
                databaseOptions = value;
                OnPropertyChanged("DatabaseOptions");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
