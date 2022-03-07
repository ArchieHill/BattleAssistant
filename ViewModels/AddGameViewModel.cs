using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Battle_Assistant.ViewModels
{
    public class AddGameViewModel
    {
        public GameModel Game { get; set; }

        //The window handle needed to initialise the file and folder pickers
        private readonly IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);

        public AddGameViewModel()
        {
            Game = new GameModel();
        }

        public async void SelectIcon()
        {
            var filePicker = new FileOpenPicker();

            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            filePicker.FileTypeFilter.Add(".png");

            StorageFile file = await filePicker.PickSingleFileAsync();
            Game.GameIcon = file.Path;
        }

        public async void SelectGameFilesDir()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            Game.GameFilesDir = folder.Path;
        }

        public GameModel AddGame()
        {
            return Game;
        }

    }
}
