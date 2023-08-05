// AddOpponentDialogModel.cs
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
using System.Threading.Tasks;
using BattleAssistant.Common.CustomValidation;
using BattleAssistant.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BattleAssistant.DialogModels
{
    /// <summary>
    /// Add Opponents Dialog Model
    /// </summary>
    public partial class AddOpponentDialogModel : ObservableValidator
    {
        [ObservableProperty]
        private bool primaryButtonEnabled; //Workaround until this fixed https://github.com/microsoft/microsoft-ui-xaml/issues/8563

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        private string opponentName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [DirectoryPath]
        //[NotifyCanExecuteChangedFor(nameof(AddOpponentCommand))]
        private string opponentSharedDirectory;

        partial void OnOpponentNameChanged(string value)
        {
            ValidateAllProperties();
            PrimaryButtonEnabled = !HasErrors;
        }

        partial void OnOpponentSharedDirectoryChanged(string value)
        {
            ValidateAllProperties();
            PrimaryButtonEnabled = !HasErrors;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddOpponentDialogModel() { }

        /// <summary>
        /// Opens a folder picker to get the user to select the shared drive folder
        /// </summary>
        /// <returns>The folders path</returns>
        [RelayCommand]
        private async Task SelectSharedDirectory(InfoBar errorInfoBar)
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                OpponentSharedDirectory = folder.Path;
            }
        }

        /// <summary>
        /// Adds the opponent to the list and updates the save file
        /// </summary>
        //[RelayCommand(CanExecute = nameof(ValidateInputs))]
        [RelayCommand]
        private void AddOpponent()
        {
            App.AddOpponent(new OpponentModel(OpponentName, OpponentSharedDirectory));
        }
    }
}
