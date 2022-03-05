using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string gameIcon;
        public string GameIcon
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

        private string gameDir;
        public string GameDir
        {
            get { return gameDir; }
            set
            {
                if (gameDir != value)
                {
                    gameDir = value;
                    NotifyPropertyChanged("GameDir");
                }
            }
        }

        private string gameFilesDir;
        public string GameFilesDir
        {
            get { return gameFilesDir; }
            set
            {
                if(gameFilesDir != value)
                {
                    gameFilesDir = value;
                    SetEmailDirs(gameFilesDir);
                    NotifyPropertyChanged("GameFilesDir");
                }
            }
        }
        private string incomingEmailDir;
        public string IncomingEmailDir
        {
            get { return incomingEmailDir; }
            set
            {
                if (incomingEmailDir != value)
                {
                    incomingEmailDir = value;
                    NotifyPropertyChanged("IncomingEmailDir");
                }
            }
        }

        private string outgoingEmailDir;
        public string OutgoingEmailDir
        {
            get { return outgoingEmailDir; }
            set
            {
                if (outgoingEmailDir != value)
                {
                    outgoingEmailDir = value;
                    NotifyPropertyChanged("OutgoingEmailDir");
                }
            }
        }

        public GameModel()
        {
            Name = "";
            GameIcon = "";
            GameDir = "";
            IncomingEmailDir = "";
            OutgoingEmailDir = "";
        }

        public GameModel(string name, string gameDir, string gameFilesDir, string gameIcon)
        {
            Name = name;
            GameIcon = gameIcon;
            GameDir = gameDir;
            GameFilesDir = gameFilesDir;
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        override
        public string ToString()
        {
            return Name;
        }

        public void SetEmailDirs(string gameFilesDir)
        {
            //A check to make sure the gamefiles dir is a gamefiles dir
            IncomingEmailDir = gameFilesDir + "/Incoming Email";
            OutgoingEmailDir = gameFilesDir + "/Outgoing Email";
        }
    }
}
