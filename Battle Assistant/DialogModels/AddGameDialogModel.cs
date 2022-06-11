using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Battle_Assistant.Models;
using Battle_Assistant.Helpers;

namespace Battle_Assistant.DialogModels
{
    /// <summary>
    /// Add Game Dialog Model
    /// </summary>
    public class AddGameDialogModel
    {
        public GameModel Game { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddGameDialogModel()
        {
            Game = new GameModel();
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
            if(folder != null)
            {
                Game.GameDir = folder.Path;
            }
        }

        /// <summary>
        /// Opens a file picker for the user to select a png file for the icon
        /// </summary>
        public async void SelectIconFile()
        {
            var filePicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, App.Hwnd);
            filePicker.FileTypeFilter.Add(".png");
            StorageFile file = await filePicker.PickSingleFileAsync();
            if(file != null)
            {
                Game.GameIcon = file.Path;
            }
        }

        /// <summary>
        /// Adds the game to the list and updates the save file
        /// </summary>
        public void AddGame()
        {
            App.Games.Add(Game);
            //Assign its index so we know where to look to delete it
            Game.Index = App.Games.IndexOf(Game);
            StorageHelper.UpdateGameFile();
        }
    }
}
