// OpponentsPageViewModel.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.ObjectModel;
using BattleAssistant.Dialogs;
using BattleAssistant.Helpers;
using BattleAssistant.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;

namespace BattleAssistant.ViewModels
{
    /// <summary>
    /// Opponents page view model
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
            AddOpponentDialog dialog = new()
            {
                XamlRoot = root
            };
            await dialog.ShowAsync();
            Log.Information("Opponent added");
        }

        /// <summary>
        /// Deletes the opponent
        /// </summary>
        /// <param name="index"></param>
        public async void DeleteOpponent(int index, XamlRoot root)
        {
            bool deleteAllowed = true;

            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Opponent.Name == Opponents[index].Name)
                {
                    deleteAllowed = false;
                    break;
                }
            }

            DeleteConfirmationDialog dialog = new(deleteAllowed)
            {
                XamlRoot = root
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Opponents[index].Dispose();
                Opponents.RemoveAt(index);
                UpdateIndexes();
                StorageHelper.UpdateOpponentFile();
                Log.Information("Opponent deleted");
            }
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
