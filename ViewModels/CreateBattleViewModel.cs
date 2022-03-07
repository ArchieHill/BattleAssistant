using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Battle_Assistant.ViewModels
{
    public class CreateBattleViewModel
    {
        private ObservableCollection<GameModel> games;
        public ObservableCollection<GameModel> Games
        {
            get { return games; }
            set { games = value; SelectedGame = value.ElementAt(0); }
        }

        public BattleModel Battle { get; set; }

        public GameModel SelectedGame { get; set; }

        //The window handle needed to initialise the file and folder pickers
        private readonly IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);

        public CreateBattleViewModel()
        {
            Battle = new BattleModel();
        }

        public async void SelectGameFile()
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.FileTypeFilter.Add(".ema");
            Battle.InitialGameFile = await filePicker.PickSingleFileAsync();
        }

        public async void SelectDrive()
        {
            var folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.FileTypeFilter.Add("*");
            Battle.SharedDir = await folderPicker.PickSingleFolderAsync();
        }


        public BattleModel CreateBattle()
        {
            Battle.Game = SelectedGame;
            return Battle;
        }
    }
}
