using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Models
{
    public class BattleModel : MasterModel
    {
        private const int FIRST_NUM_POS_SUBTRACTOR = -3;

        private const int LAST_FILE_NAME_POS_SUBTRACTOR = -4;

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

        private StorageFile initialGameFile;
        public StorageFile InitialGameFile
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

        public OpponentModel Opponent { get; set; }

        public BattleModel() : base()
        {
            Name = "";
            CurrentFileNum = 1;
            Game = null;
            Opponent = null;
            Status = "No Status Set";
            LastAction = "No Last Action";
        }

        public BattleModel(StorageFile gameFile, GameModel game, OpponentModel opponent)
        {
            SetVarsFromGameFile(gameFile);
            Game = game;
            Opponent = opponent;
            Status = "No Status Set";
            LastAction = "No Last Action";
        }

        public void SetVarsFromGameFile(StorageFile gameFile)
        {
            string filename = gameFile.DisplayName;
            Name = filename.Substring(0, filename.Length + LAST_FILE_NAME_POS_SUBTRACTOR);
            CurrentFileNum = int.Parse(filename.Substring(filename.Length + FIRST_NUM_POS_SUBTRACTOR));
        }
    }
}
