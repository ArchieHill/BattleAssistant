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
using System.ComponentModel;
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
    public class StartBattleDialogModel : INotifyPropertyChanged
    {

        // A property changed event object
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the page when a property has changed so the view can be updated
        /// </summary>
        /// <param name="propName">The property name</param>
        protected void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private GameModel selectedGame;
        public GameModel SelectedGame
        {
            get { return selectedGame; }
            set

            {
                if (selectedGame != value)
                {
                    selectedGame = value;
                    NotifyPropertyChanged("SelectedGame");
                }
            }
        }

        private OpponentModel selectedOpponent;
        public OpponentModel SelectedOpponent
        {
            get { return selectedOpponent; }
            set

            {
                if (selectedOpponent != value)
                {
                    selectedOpponent = value;
                    NotifyPropertyChanged("SelectedOpponent");
                }
            }
        }

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
                if (!FileHelper.CheckFileIsValid(file.Path))
                {
                    dialogInfoBar.Severity = InfoBarSeverity.Error;
                    dialogInfoBar.Title = "Invalid file";
                    dialogInfoBar.Message = "The file name doesn't end with three numbers e.g 001";
                    dialogInfoBar.IsOpen = true;
                }

                Battle.BattleFile = file.Path;

                string fileDir = Path.GetDirectoryName(file.Path);
                if (Path.GetFileName(fileDir) == "Outgoing Email" || Path.GetFileName(fileDir) == "Incoming Email")
                {
                    if (SettingsHelper.GetAutoSelectGame())
                    {
                        AutoSelectGame(fileDir, file.Path);
                    }
                }
                else
                {
                    if (SettingsHelper.GetAutoSelectOpponent())
                    {
                        AutoSelectOpponent(fileDir, file.Path);
                    }
                }
            }
        }

        /// <summary>
        /// Auto selects a game if the battle file is in a game folder
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="path"></param>
        private void AutoSelectGame(string fileDir, string path)
        {
            //Try and find if the game already exists for the game directory found
            foreach (GameModel game in Games)
            {
                if (fileDir.Contains(game.Name))
                {
                    SelectedGame = game;
                    return;
                }
            }

            if (SettingsHelper.GetAutoCreateGame())
            {
                //If the game can't be found create a new game
                GameModel newGame = new GameModel(Path.GetDirectoryName(Path.GetDirectoryName(fileDir)));
                App.AddGame(newGame);
                SelectedGame = newGame;
            }
        }

        /// <summary>
        /// Auto selects an opponent if its not in a game folder
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="path"></param>
        private void AutoSelectOpponent(string fileDir, string path)
        {
            //Try and find if the opponent already exists for this shared directory
            foreach (OpponentModel opponent in Opponents)
            {
                if (opponent.SharedDir == fileDir)
                {
                    SelectedOpponent = opponent;
                    return;
                }
            }

            if (SettingsHelper.GetAutoCreateOpponent())
            {
                //If the opponent can't be found create a new opponent
                OpponentModel newOpponent = new OpponentModel(Path.GetFileName(fileDir), fileDir);
                App.AddOpponent(newOpponent);
                SelectedOpponent = newOpponent;
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
