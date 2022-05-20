using Battle_Assistant.Models;
using Battle_Assistant.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Assistant.Dialogs;
using Microsoft.UI.Xaml;

namespace Battle_Assistant.ViewModels
{
    public sealed class BattlesPageViewModel
    {
        public ObservableCollection<BattleModel> Battles { get; set; } = App.Battles;

        public BattlesPageViewModel()
        {

        }
        public async void StartBattle(XamlRoot root)
        {
            StartBattleDialog dialog = new StartBattleDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }
    }
}
