using Battle_Assistant.Watchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Models
{
    public class OpponentModel : MasterModel
    {
        private SharedDriveWatcher sDWatcher;

        private StorageFolder sharedDir;
        public StorageFolder SharedDir
        {
            get { return sharedDir; }
            set
            {
                if (sharedDir != value)
                {
                    sharedDir = value;
                    sDWatcher = new SharedDriveWatcher(sharedDir);
                    NotifyPropertyChanged("SharedDir");
                }
            }
        }

        public OpponentModel() : base()
        {
            SharedDir = null;
        }
    }
}
