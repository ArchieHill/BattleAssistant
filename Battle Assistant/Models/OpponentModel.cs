using Battle_Assistant.Watchers;
using System;

namespace Battle_Assistant.Models
{
    /// <summary>
    /// Opponent model
    /// </summary>
    public class OpponentModel : MasterModel, IDisposable
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

        /// <summary>
        /// Constructor
        /// </summary>
        public OpponentModel() : base()
        {
            SharedDir = null;
        }

        public void Dispose()
        {
            sDWatcher.Dispose();
        }
    }
}
