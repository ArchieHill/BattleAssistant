using Battle_Assistant.Watchers;
using System;
using System.IO;

namespace Battle_Assistant.Models
{
    /// <summary>
    /// The game model
    /// </summary>
    public class GameModel : MasterModel, IDisposable
    {
        private OutGoingEmailFolderWatcher oGEFWatcher;

        private string gameDir;
        public string GameDir
        {
            get { return gameDir; }
            set
            {
                if (gameDir != value)
                {
                    gameDir = value;
                    Name = Path.GetFileNameWithoutExtension(value);
                    SetEmailFolders();
                    NotifyPropertyChanged("GameDir");
                }
            }
        }

        private string incomingEmailFolder;
        public string IncomingEmailFolder
        {
            get { return incomingEmailFolder; }
            set
            {
                if (incomingEmailFolder != value)
                {
                    incomingEmailFolder = value;
                    NotifyPropertyChanged("IncomingEmailFolder");
                }
            }
        }

        private string outgoingEmailFolder;
        public string OutgoingEmailFolder
        {
            get { return outgoingEmailFolder; }
            set
            {
                if (outgoingEmailFolder != value)
                {
                    outgoingEmailFolder = value;
                    oGEFWatcher = new OutGoingEmailFolderWatcher(outgoingEmailFolder);
                    NotifyPropertyChanged("OutgoingEmailFolder");
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameModel() : base()
        {
            GameDir = null;
            IncomingEmailFolder = null;
            OutgoingEmailFolder = null;
        }

        /// <summary>
        /// Sets the email folders from the game directory folder
        /// </summary>
        public void SetEmailFolders()
        {
            IncomingEmailFolder = GameDir + "\\Game Files\\Incoming Email";
            OutgoingEmailFolder = GameDir + "\\Game Files\\Outgoing Email";
        }

        public void Dispose()
        {
            oGEFWatcher.Dispose();
        }
    }
}
