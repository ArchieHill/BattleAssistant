using CM_Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.ViewModels
{    
    public class BattlesPageViewModel
    {
        public ObservableCollection<BattleModel> Battles { get; set; }

        public BattlesPageViewModel()
        {
            Battles = new ObservableCollection<BattleModel>();

            GameModel game = new GameModel("example game", "game directory", "game files directory", "game icon");
            Battles.Add(new BattleModel("Example gamefile 001.ema", "shared directory", game, "opponent"));
            Battles.Add(new BattleModel());
            Battles.Add(new BattleModel());
        }
    }
}
