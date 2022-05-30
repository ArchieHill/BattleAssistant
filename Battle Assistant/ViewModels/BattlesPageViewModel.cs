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
    /// <summary>
    /// Battle page view model
    /// </summary>
    public sealed class BattlesPageViewModel
    {
        public ObservableCollection<BattleModel> Battles { get; set; } = App.Battles;

        /// <summary>
        /// Constructor
        /// </summary>
        public BattlesPageViewModel()
        {

        }

        /// <summary>
        /// Opens the start battle dialog
        /// </summary>
        /// <param name="root">The window root for the dialog</param>
        public async void StartBattle(XamlRoot root)
        {
            StartBattleDialog dialog = new StartBattleDialog();
            dialog.XamlRoot = root;
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Deletes the battle from the list
        /// </summary>
        /// <param name="index"></param>
        public async void EndBattle(int index, XamlRoot root)
        {
            EndBattleConfirmationDialog dialog = new EndBattleConfirmationDialog();
            dialog.XamlRoot = root;
            var result = await dialog.ShowAsync();
            if(result == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                Battles.RemoveAt(index);
                UpdateIndexes();
                StorageHelper.UpdateBattleFile();
            }
        }

        /// <summary>
        /// Update the battles indexs with their new indexes
        /// Needs to be called when the list has been manipulated
        /// </summary>
        private void UpdateIndexes()
        {
            for (int i = 0; i < Battles.Count; i++)
            {
                Battles[i].Index = i;
            }
        }
    }
}
