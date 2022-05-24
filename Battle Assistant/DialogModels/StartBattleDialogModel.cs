using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Assistant.Models;
using Windows.Storage.Pickers;

namespace Battle_Assistant.DialogModels
{
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

        public StartBattleDialogModel()
        {
            Battle = new BattleModel();
            Games = App.Games;
            Opponents = App.Opponents;
        }

        public async Task SelectBattleFile()
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, App.Hwnd);
            filePicker.FileTypeFilter.Add(".ema");
            Battle.InitialGameFile = await filePicker.PickSingleFileAsync();
        }

        public void StartBattle()
        {
            Battle.Game = SelectedGame;
            Battle.Opponent = SelectedOpponent;
            App.Battles.Add(Battle);
            //Assign its index so we know where to look to delete it
            Battle.Index = App.Battles.IndexOf(Battle);
        }
    }
}
