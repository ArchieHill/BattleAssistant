using Battle_Assistant.Dialogs;
using Battle_Assistant.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.ViewModels
{
    public class OpponentsPageViewModel
    {
        public ObservableCollection<OpponentModel> Opponents { get; set; } = App.Opponents;

        public OpponentsPageViewModel()
        {

        }

        public async void AddOpponent(XamlRoot root)
        {
            AddOpponentDialog dialog = new AddOpponentDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }
    }
}
