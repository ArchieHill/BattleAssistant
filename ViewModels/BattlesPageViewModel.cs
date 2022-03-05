using Battle_Assistant.Models;
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
            Battles = new ObservableCollection<BattleModel>();
        }
    }
}
