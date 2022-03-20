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
        public ObservableCollection<BattleModel> Battles { get; set; } = App.Battles;

        public BattlesPageViewModel()
        {

        }
    }
}
