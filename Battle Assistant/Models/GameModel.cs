using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using Battle_Assistant.Watchers;

namespace Battle_Assistant.Models
{
    public class GameModel : MasterModel
    {
        private OutGoingEmailFolderWatcher oGEFWatcher;

        private StorageFile gameIcon;
        public StorageFile GameIcon
        {
            get { return gameIcon; }
            set
            {
                if (gameIcon != value)
                {
                    gameIcon = value;
                    NotifyPropertyChanged("GameIcon");
                }
            }
        }

        private StorageFolder gameDir;
        public StorageFolder GameDir
        {
            get { return gameDir; }
            set
            {
                if (gameDir != value)
                {
                    gameDir = value;
                    Name = value.Name;
                    SetEmailFolders();
                    NotifyPropertyChanged("GameDir");
                }
            }
        }

        private StorageFolder incomingEmailFolder;
        public StorageFolder IncomingEmailFolder
        {
            get { return incomingEmailFolder; }
            set
            {
                if (incomingEmailFolder != value)
                {
                    incomingEmailFolder = value;
                    NotifyPropertyChanged("IncomingEmailDir");
                }
            }
        }

        private StorageFolder outgoingEmailFolder;
        public StorageFolder OutgoingEmailFolder
        {
            get { return outgoingEmailFolder; }
            set
            {
                if (outgoingEmailFolder != value)
                {
                    outgoingEmailFolder = value;
                    oGEFWatcher = new OutGoingEmailFolderWatcher(outgoingEmailFolder);
                    NotifyPropertyChanged("OutgoingEmailDir");
                }
            }
        }

        public GameModel()
        {
            Name = "";
            GameIcon = null;
            GameDir = null;
            IncomingEmailFolder = null;
            OutgoingEmailFolder = null;
        }

        public GameModel(string name, StorageFolder gameDir, StorageFile gameIcon)
        {
            Name = name;
            GameIcon = gameIcon;
            GameDir = gameDir;
        }

        public async void SetEmailFolders()
        {
            IncomingEmailFolder = await StorageFolder.GetFolderFromPathAsync(GameDir.Path + "\\Game Files\\Incoming Email");
            OutgoingEmailFolder = await StorageFolder.GetFolderFromPathAsync(GameDir.Path + "\\Game Files\\Outgoing Email");
        }
    }
}
