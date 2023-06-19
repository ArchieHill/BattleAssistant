// AddGameDialogModel.cs
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BattleAssistant.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BattleAssistant.DialogModels
{
    /// <summary>
    /// Add Game Dialog Model
    /// </summary>
    public partial class AddGameDialogModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddGameCommand))]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(5)]
        private string gameDirectory = string.Empty;

        partial void OnGameDirectoryChanged(string value)
        {
            IsValid = !HasErrors;
        }

        [ObservableProperty]
        private bool isValid; 

        /// <summary>
        /// Constructor
        /// </summary>
        public AddGameDialogModel() {}

        /// <summary>
        /// Opens a folder picker for the user to select the games directory
        /// </summary>
        /// <returns>The path of the folder</returns>
        [RelayCommand]
        private async Task SelectGameDirectory(InfoBar errorInfoBar)
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            //Validate folder path
            if (folder != null)
            {
                if (!Directory.Exists($@"{folder.Path}\Game Files"))
                {
                    errorInfoBar.Severity = InfoBarSeverity.Error;
                    errorInfoBar.Title = "Invalid folder";
                    errorInfoBar.Message = "The folder selected doesn't contain the Game Files folder";
                    errorInfoBar.IsOpen = true;
                    return;
                }

                if (!Directory.Exists($@"{folder.Path}\Game Files\Incoming Email"))
                {
                    errorInfoBar.Severity = InfoBarSeverity.Error;
                    errorInfoBar.Title = "Invalid folder";
                    errorInfoBar.Message = "The folder selected doesn't contain an Incoming Email folder in the Game Files folder";
                    errorInfoBar.IsOpen = true;
                    return;
                }

                if (!Directory.Exists($@"{folder.Path}\Game Files\Outgoing Email"))
                {
                    errorInfoBar.Severity = InfoBarSeverity.Error;
                    errorInfoBar.Title = "Invalid folder";
                    errorInfoBar.Message = "The folder selected doesn't contain an Outgoing Email folder in the Game Files folder";
                    errorInfoBar.IsOpen = true;
                    return;
                }

                GameDirectory = folder.Path;
            }
        }

        /// <summary>
        /// Adds the game to the list and updates the save file
        /// </summary>
        [RelayCommand(CanExecute = nameof(IsValid))]
        private void AddGame()
        {
            App.AddGame(new GameModel(GameDirectory));
        }
    }
}
