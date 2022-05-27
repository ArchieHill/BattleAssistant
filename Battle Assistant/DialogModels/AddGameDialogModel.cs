using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Battle_Assistant.Models;
using Battle_Assistant.Helpers;

namespace Battle_Assistant.DialogModels
{
    public class AddGameDialogModel
    {
        public GameModel Game { get; set; }

        public AddGameDialogModel()
        {
            Game = new GameModel();
        }

        public async Task SelectGameDir()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            Game.GameDir = folder.Path;
        }

        public async void SelectIconFile()
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, App.Hwnd);
            filePicker.FileTypeFilter.Add(".png");
            StorageFile file = await filePicker.PickSingleFileAsync();
            Game.GameIcon = file.Path;
        }
        public void AddGame()
        {
            App.Games.Add(Game);
            //Assign its index so we know where to look to delete it
            Game.Index = App.Games.IndexOf(Game);
            StorageHelper.UpdateGameFile();
        }
    }
}
