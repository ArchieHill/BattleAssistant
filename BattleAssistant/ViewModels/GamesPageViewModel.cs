// GamesPageViewModel.cs
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

namespace BattleAssistant.ViewModels
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
            AddGameDialog dialog = new()
            {
                XamlRoot = root
            };
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Deletes the game from the list
        /// </summary>
        /// <param name="index"></param>
        public async void DeleteGame(int index, XamlRoot root)
        {
            bool deleteAllowed = true;

            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Game.Name == Games[index].Name)
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
