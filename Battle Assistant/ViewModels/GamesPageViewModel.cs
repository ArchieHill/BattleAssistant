using Battle_Assistant.Dialogs;
using Battle_Assistant.Helpers;
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
    public class GamesPageViewModel
    {
        public ObservableCollection<GameModel> Games { get; set; } = App.Games;

        public GamesPageViewModel()
        {

        }

        public async void AddGame(XamlRoot root)
        {
            AddGameDialog dialog = new AddGameDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }
    }
}
