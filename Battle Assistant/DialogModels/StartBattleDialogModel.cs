// StartBattleDialogModel.cs
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Battle_Assistant.DialogModels
{
    /// <summary>
    /// Start Battle Dialog Model
    /// </summary>
    public class StartBattleDialogModel
    {
        public GameModel SelectedGame { get; set; }

        public OpponentModel SelectedOpponent { get; set; }

        private ObservableCollection<GameModel> games;

        public ObservableCollection<GameModel> Games
        {
            get { return games; }
            set
            {
                games = value;
                if (value.Count > 0)
                {
                    SelectedGame = value.ElementAt(0);
                }
            }
        }

        private ObservableCollection<OpponentModel> opponents;

        public ObservableCollection<OpponentModel> Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
                if (value.Count > 0)
                {
                    SelectedOpponent = value.ElementAt(0);
                }
            }
        }

        public BattleModel Battle { get; set; }

        private InfoBar dialogInfoBar;

        /// <summary>
        /// Constructor
        /// </summary>
        public StartBattleDialogModel(InfoBar dialogInfoBar)
        {
            Battle = new BattleModel();
            Games = App.Games;
            Opponents = App.Opponents;
            this.dialogInfoBar = dialogInfoBar;
        }

        /// <summary>
        /// Opens a file picker to so the user can select the battle file
        /// </summary>
        /// <returns>The battle files path</returns>
        public async Task SelectBattleFile()
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, App.Hwnd);
            filePicker.FileTypeFilter.Add(".ema");
            StorageFile file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                if (FileHelper.CheckFileIsValid(file.Path))
                {
                    Battle.BattleFile = file.Path;
                }
                else
                {
                    dialogInfoBar.Severity = InfoBarSeverity.Error;
                    dialogInfoBar.Title = "Invalid file";
                    dialogInfoBar.Message = "The file name doesn't end with three numbers e.g 001";
                    dialogInfoBar.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// Adds the battle to the list and updates the save file
        /// </summary>
        public void StartBattle()
        {
            Battle.Game = SelectedGame;
            Battle.Opponent = SelectedOpponent;
            App.Battles.Add(Battle);
            //Assign its index so we know where to look to delete it
            Battle.Index = App.Battles.IndexOf(Battle);
            if (File.Exists(Battle.Game.OutgoingEmailFolder + "\\" + Path.GetFileName(Battle.BattleFile)))
            {
                FileHelper.CopyToSharedDrive(Battle);
            }
            else
            {
                FileHelper.CopyToIncomingEmail(Battle);
            }
        }
    }
}
