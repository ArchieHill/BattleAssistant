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
using System.IO;
using System.Threading.Tasks;
using Battle_Assistant.Models;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Battle_Assistant.DialogModels
{
    /// <summary>
    /// Add Game Dialog Model
    /// </summary>
    public class AddGameDialogModel
    {
        public GameModel Game { get; set; }

        private InfoBar dialogInfoBar;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddGameDialogModel(InfoBar dialogInfoBar)
        {
            Game = new GameModel();
            this.dialogInfoBar = dialogInfoBar;
        }

        /// <summary>
        /// Opens a folder picker for the user to select the games directory
        /// </summary>
        /// <returns>The path of the folder</returns>
        public async Task SelectGameDir()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                if (Directory.Exists(folder.Path + "\\Game Files"))
                {
                    Game.GameDir = folder.Path;
                }
                else
                {
                    dialogInfoBar.Severity = InfoBarSeverity.Error;
                    dialogInfoBar.Title = "Invalid folder";
                    dialogInfoBar.Message = "The folder selected doesn't contain the Game Files folder";
                    dialogInfoBar.IsOpen = true;
                }

            }
        }

        /// <summary>
        /// Adds the game to the list and updates the save file
        /// </summary>
        public void AddGame()
        {
            App.AddGame(Game);
        }
    }
}
