// SettingsPageViewModel.cs
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
using System.Threading.Tasks;
using BattleAssistant.Common;
using BattleAssistant.Interfaces;
using BattleAssistant.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BattleAssistant.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly ISettingsService SettingsService;

        [ObservableProperty]
        private bool flashAmountBoxEnabled;

        [ObservableProperty]
        private bool autoCreateOpponentEnabled;

        [ObservableProperty]
        private bool autoCreateGameEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPageViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;

            AutoSelectOpponent = SettingsService.GetAutoSelectOpponent();
            AutoCreateOpponent = SettingsService.GetAutoCreateOpponent();
            AutoSelectGame = SettingsService.GetAutoSelectGame();
            AutoCreateGame = SettingsService.GetAutoCreateGame();

            FlashIcon = SettingsService.GetFlashIcon();
            FlashAmount = SettingsService.GetFlashAmount();

            BackupFolderPath = SettingsService.GetBackupFolderPath();
        }


        /// <summary>
        /// Opens a folder picker to get the user to select the shared drive folder
        /// </summary>
        /// <returns>The folders path</returns>
        [RelayCommand]
        public async Task SelectBackupFolder()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                BackupFolderPath = folder.Path;
            }
        }

        [ObservableProperty]
        private bool flashIcon;

        partial void OnFlashIconChanged(bool value)
        {
            SettingsService.SaveFlashIcon(value);
            FlashAmountBoxEnabled = value;
        }

        [ObservableProperty]
        private int flashAmount;

        partial void OnFlashAmountChanged(int value)
        {
            SettingsService.SaveFlashAmount(value);
        }

        [ObservableProperty]
        private bool autoSelectOpponent;

        partial void OnAutoSelectOpponentChanged(bool value)
        {
            SettingsService.SaveAutoSelectOpponent(value);
            AutoCreateOpponentEnabled = value;
            if(!value)
            {
                AutoCreateOpponent = false;
                AutoCreateGame = false;
            }
        }

        [ObservableProperty]
        private bool autoCreateOpponent;
        partial void OnAutoCreateOpponentChanged(bool value)
        {
            SettingsService.SaveAutoCreateOpponent(value);
        }

        [ObservableProperty]
        private bool autoSelectGame;

        partial void OnAutoSelectGameChanged(bool value)
        {
            SettingsService.SaveAutoSelectGame(value);
            AutoCreateGameEnabled = value;
            if (!value)
            {
                AutoCreateGame = false;
            }
        }

        [ObservableProperty]
        private bool autoCreateGame;
        partial void OnAutoCreateGameChanged(bool value)
        {
            SettingsService.SaveAutoCreateGame(value);
        }

        [ObservableProperty]
        private string backupFolderPath;

        partial void OnBackupFolderPathChanged(string value)
        {
            SettingsService.SaveBackupFolderPath(value);
        }
    }
}
