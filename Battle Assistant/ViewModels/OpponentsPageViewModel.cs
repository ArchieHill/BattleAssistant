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
    /// <summary>
    /// Opponent page view model
    /// </summary>
    public class OpponentsPageViewModel
    {
        public ObservableCollection<OpponentModel> Opponents { get; set; } = App.Opponents;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpponentsPageViewModel()
        {

        }

        /// <summary>
        /// Opens the add opponent dialog
        /// </summary>
        /// <param name="root"></param>
        public async void AddOpponent(XamlRoot root)
        {
            AddOpponentDialog dialog = new AddOpponentDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Deletes the opponent
        /// </summary>
        /// <param name="index"></param>
        public void DeleteOpponent(int index)
        {
            Opponents.RemoveAt(index);
            UpdateIndexes();
            StorageHelper.UpdateOpponentFile();
        }

        /// <summary>
        /// Update the opponents indexs with their new indexes
        /// Needs to be called when the list has been manipulated
        /// </summary>
        private void UpdateIndexes()
        {
            for (int i = 0; i < Opponents.Count; i++)
            {
                Opponents[i].Index = i;
            }
        }
    }
}
