using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
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
                if(value.Count > 0) 
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
                if(value.Count > 0)
                {
                    SelectedOpponent = value.ElementAt(0);
                }
            }
        }
        public BattleModel Battle { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public StartBattleDialogModel()
        {
            Battle = new BattleModel();
            Games = App.Games;
            Opponents = App.Opponents;
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
            Battle.BattleFile = file.Path;
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
            if(File.Exists(Battle.Game.OutgoingEmailFolder + "\\" + Path.GetFileName(Battle.BattleFile)))
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
