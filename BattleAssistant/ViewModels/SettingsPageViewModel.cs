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
using BattleAssistant.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BattleAssistant.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private NumberBox flashAmountBox;

        private ToggleSwitch autoCreateOpponentSwitch;

        private ToggleSwitch autoCreateGameSwitch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="autoCreateOpponentSwitch"></param>
        /// <param name="autoCreateGameSwitch"></param>
        public SettingsPageViewModel(NumberBox flashAmountBox, ToggleSwitch autoCreateOpponentSwitch, ToggleSwitch autoCreateGameSwitch)
        {
            this.flashAmountBox = flashAmountBox;
            this.autoCreateOpponentSwitch = autoCreateOpponentSwitch;
            this.autoCreateGameSwitch = autoCreateGameSwitch;
            BackupFolderPath = SettingsHelper.GetBackupFolderPath();
        }


        /// <summary>
        /// Opens a folder picker to get the user to select the shared drive folder
        /// </summary>
        /// <returns>The folders path</returns>
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
            SettingsHelper.SaveFlashIcon(value);
            flashAmountBox.IsEnabled = value;
        }

        [ObservableProperty]
        private int flashAmount;

        partial void OnFlashAmountChanged(int value)
        {
            SettingsHelper.SaveFlashAmount(value);
        }

        [ObservableProperty]
        private bool autoSelectOpponent;

        partial void OnAutoSelectOpponentChanged(bool value)
        {
            SettingsHelper.SaveAutoSelectOpponent(value);
            autoCreateOpponentSwitch.IsEnabled = value;
            if(!value)
            {
                AutoCreateOpponent = false;
                autoCreateGameSwitch.IsOn = false;
            }
        }

        [ObservableProperty]
        private bool autoCreateOpponent;
        partial void OnAutoCreateOpponentChanged(bool value)
        {
            SettingsHelper.SaveAutoCreateOpponent(value);
        }

        [ObservableProperty]
        private bool autoSelectGame;

        partial void OnAutoSelectGameChanged(bool value)
        {
            SettingsHelper.SaveAutoSelectGame(value);
            autoCreateGameSwitch.IsEnabled = value;
            if (!value)
            {
                AutoCreateGame = false;
                autoCreateGameSwitch.IsOn = false;
            }
        }

        [ObservableProperty]
        private bool autoCreateGame;
        partial void OnAutoCreateGameChanged(bool value)
        {
            SettingsHelper.SaveAutoCreateGame(value);
        }

        [ObservableProperty]
        private string backupFolderPath;

        partial void OnBackupFolderPathChanged(string oldValue)
        {
            SettingsHelper.SaveBackupFolderPath(oldValue);
        }
    }
}
