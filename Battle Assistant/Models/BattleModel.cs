using Battle_Assistant.Common;
using Battle_Assistant.Helpers;
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

        private string battleFile;
        public string BattleFile
        {
            get { return battleFile; }
            set
            {
                if(battleFile != value)
                {
                    battleFile = value;
                    SetVarsFromGameFile(battleFile);
                    NotifyPropertyChanged("BattleFile");
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
            Status = Common.Status.NO_STATUS;
            LastAction = Actions.NO_LAST_ACTION;
        }

        public void SetVarsFromGameFile(string gameFile)
        {
            Name = FileHelper.GetFileDisplayName(gameFile);
            CurrentFileNum = FileHelper.GetFileNumber(gameFile);
        }
    }
}
