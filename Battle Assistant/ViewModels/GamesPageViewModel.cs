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

        public void DeleteGame(int index)
        {
            Games.RemoveAt(index);
            UpdateIndexes();
        }

        /// <summary>
        /// Update the games indexs with their new indexes
        /// Needs to be called when the list has been manipulated
        /// </summary>
        private void UpdateIndexes()
        {
            for (int i = 0; i < Games.Count; i++)
            {
                Games[i].Index = i;
            }
        }
    }
}
