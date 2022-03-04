using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Battle_Assistant.Models
{
    public class BattleModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int FIRST_NUM_POS_SUBTRACTOR = -3;

        private const int LAST_FILE_NAME_POS_SUBTRACTOR = -4;

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

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private string opponent;
        public string Opponent
        {
            get { return opponent; }
            set
            {
                if (opponent != value)
                {
                    opponent = value;
                    NotifyPropertyChanged("Opponent");

                }
            }
        }

        private string lastAction;
        public string LastAction
        {
            get { return lastAction; }
            set
            {
                if (lastAction != value)
                {
                    lastAction = value;
                    NotifyPropertyChanged("LastAction");
                }
            }
        }

        private string sharedDir;
        public string SharedDir
        {
            get { return sharedDir; }
            set
            {
                if (sharedDir != value)
                {
                    sharedDir = value;
                    NotifyPropertyChanged("SharedDir");
                }
            }
        }

        private string initialGameFile;

        public string InitialGameFile
        {
            get { return initialGameFile; }
            set
            {
                if(initialGameFile != value)
                {
                    initialGameFile = value;
                    SetVarsFromGameFile(initialGameFile);
                    NotifyPropertyChanged("InitialGameFile");
                }
            }
        }
        private int currentFileNum;
        public int CurrentFileNum
        {
            get { return currentFileNum; }
            set
            {
                if (currentFileNum != value)
                {
                    currentFileNum = value;
                    NotifyPropertyChanged("CurrentFileNum");
                }
            }
        }

        public GameModel Game { get; set; }

        public BattleModel()
        {
            Name = "";
            CurrentFileNum = 1;
            SharedDir = "";
            Game = null;
            Opponent = "";
            Status = "No Status Set";
            LastAction = "No Last Action";
        }

        public BattleModel(string gameFile, string sharedDir, GameModel game, string opponent)
        {
            SetVarsFromGameFile(gameFile);
            SharedDir = sharedDir;
            Game = game;
            Opponent = opponent;
            Status = "No Status Set";
            LastAction = "No Last Action";
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void SetVarsFromGameFile(string filePath)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath);
            Name = filename.Substring(0, filename.Length + LAST_FILE_NAME_POS_SUBTRACTOR);
            CurrentFileNum = int.Parse(filename.Substring(filename.Length + FIRST_NUM_POS_SUBTRACTOR));
        }
    }
}
