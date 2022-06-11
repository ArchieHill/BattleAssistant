using Battle_Assistant.Dialogs;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.ViewModels
{
    /// <summary>
    /// Games page view model
    /// </summary>
    public class GamesPageViewModel
    {
        public ObservableCollection<GameModel> Games { get; set; } = App.Games;

        /// <summary>
        /// Constructor
        /// </summary>
        public GamesPageViewModel()
        {

        }

        /// <summary>
        /// Opens the add game dialog
        /// </summary>
        /// <param name="root"></param>
        public async void AddGame(XamlRoot root)
        {
            AddGameDialog dialog = new AddGameDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Deletes the game from the list
        /// </summary>
        /// <param name="index"></param>
        public async void DeleteGame(int index, XamlRoot root)
        {
            DeleteConfirmationDialog dialog = new DeleteConfirmationDialog();
            dialog.XamlRoot = root;
            var result = await dialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                Games[index].Dispose();
                Games.RemoveAt(index);
                UpdateIndexes();
                StorageHelper.UpdateGameFile();
            }
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
