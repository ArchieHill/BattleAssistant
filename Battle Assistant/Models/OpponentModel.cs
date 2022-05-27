using Battle_Assistant.Common;
using Battle_Assistant.Helpers;
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

        private string sharedDir;
        public string SharedDir
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
