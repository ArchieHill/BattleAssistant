// BattlesPageViewModel.cs
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
using System.Threading.Tasks;
using BattleAssistant.Dialogs;
using BattleAssistant.Helpers;
using BattleAssistant.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;

namespace BattleAssistant.ViewModels
{
    /// <summary>
    /// Battle page view model
    /// </summary>
    public partial class BattlesPageViewModel
    {
        private readonly XamlRoot root;

        public ObservableCollection<BattleModel> Battles { get; set; } = App.Battles;

        /// <summary>
        /// Constructor
        /// </summary>
        public BattlesPageViewModel()
        {
            root = App.MainWindow.Content.XamlRoot;
        }

        /// <summary>
        /// Opens the start battle dialog
        /// </summary>
        [RelayCommand]
        private async void StartBattle()
        {
            StartBattleDialog dialog = new StartBattleDialog
            {
                XamlRoot = root
            };
            var result = await dialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                Log.Information("Battle started");
            }   
        }

        /// <summary>
        /// Deletes the battle from the list
        /// </summary>
        /// <param name="index">The index of the battle in the collection</param>
        [RelayCommand]
        private async void EndBattle(int index)
        {
            EndBattleConfirmationDialog dialog = new(Battles[index])
            {
                XamlRoot = root
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Battles.RemoveAt(index);
                UpdateIndexes();
                StorageHelper.UpdateBattleFile();
                Log.Information("Battle ended");
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
