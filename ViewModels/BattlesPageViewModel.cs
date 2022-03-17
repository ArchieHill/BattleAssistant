using Battle_Assistant.Models;
using Battle_Assistant.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.ViewModels
{    
    public sealed class BattlesPageViewModel
    {
        private static BattlesPageViewModel instance = null;
        private static readonly object padlock = new object();

        private const string BATTLES_FILE_NAME = "battles.json";

        public static BattlesPageViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BattlesPageViewModel();
                    }
                }

                return instance;
            }
        }
        public ObservableCollection<BattleModel> Battles { get; set; }

        BattlesPageViewModel()
        {
            Battles = StorageHelper.LoadModelsFromJSON<BattleModel>(BATTLES_FILE_NAME).Result;
        }
    }
}
