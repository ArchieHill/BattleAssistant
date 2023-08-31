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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BattleAssistant.Common.CustomValidation;
using BattleAssistant.Interfaces;
using BattleAssistant.Models;
using BattleAssistant.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BattleAssistant.DialogModels
{
    /// <summary>
    /// Start Battle Dialog Model
    /// </summary>
    public partial class StartBattleDialogModel : ObservableValidator
    {
        private readonly IFileService FileService;
        private readonly IStorageService StorageService;
        private readonly ISettingsService SettingsService;

        [ObservableProperty]
        private bool primaryButtonEnabled; //Workaround until this fixed https://github.com/microsoft/microsoft-ui-xaml/issues/8563

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        private GameModel selectedGame;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        private OpponentModel selectedOpponent;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [BattleFile]
        private string battleFile;

        [ObservableProperty]
        private bool backup;

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

        public StartBattleDialogModel(IFileService fileService, IStorageService storageService, ISettingsService settingsService)
        {
            FileService = fileService;
            StorageService = storageService;
            SettingsService = settingsService;
        }

        partial void OnBattleFileChanged(string value)
        {
            ValidateAllProperties();
            PrimaryButtonEnabled = !HasErrors;
        }

        partial void OnSelectedGameChanged(GameModel value)
        {
            ValidateAllProperties();
            PrimaryButtonEnabled = !HasErrors;
        }

        partial void OnSelectedOpponentChanged(OpponentModel value)
        {
            ValidateAllProperties();
            PrimaryButtonEnabled = !HasErrors;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StartBattleDialogModel()
        {
            Games = StorageService.Games;
            Opponents = StorageService.Opponents;
        }

        /// <summary>
        /// Opens a file picker to so the user can select the battle file
        /// </summary>
        /// <returns>The battle files path</returns>
        [RelayCommand]
        private async Task SelectBattleFile(InfoBar errorInfoBar)
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, App.Hwnd);
            filePicker.FileTypeFilter.Add(".ema");
            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                BattleFile = file.Path;

                string fileDir = Path.GetDirectoryName(file.Path);
                if (Path.GetFileName(fileDir) == "Outgoing Email" || Path.GetFileName(fileDir) == "Incoming Email")
                {
                    if (SettingsService.GetAutoSelectGame())
                    {
                        AutoSelectGame(fileDir);
                    }
                }
                else if (SettingsService.GetAutoSelectOpponent())
                {
                    AutoSelectOpponent(fileDir);
                }
            }
        }

        /// <summary>
        /// Auto selects a game if the battle file is in a game folder
        /// </summary>
        /// <param name="fileDir"></param>
        private void AutoSelectGame(string fileDir)
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

            if (SettingsService.GetAutoCreateGame())
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
        private void AutoSelectOpponent(string fileDir)
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

            if (SettingsService.GetAutoCreateOpponent())
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
        [RelayCommand]
        private async Task StartBattle()
        {
            BattleModel battle = new(BattleFile, SelectedGame, SelectedOpponent, Backup);
            StorageService.Battles.Add(battle);

            //Assign its index so we know where to look to delete it
            battle.Index = StorageService.Battles.IndexOf(battle);

            if (File.Exists($@"{battle.Game.OutgoingEmailFolder}\{Path.GetFileName(battle.BattleFile)}"))
            {
                await FileService.CopyToSharedDriveAsync(battle);
            }
            else
            {
                await FileService.CopyToIncomingEmailAsync(battle);
            }
        }
    }
}
