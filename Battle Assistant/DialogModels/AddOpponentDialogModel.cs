using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Battle_Assistant.DialogModels
{
    /// <summary>
    /// Add Opponent Dialog Model
    /// </summary>
    public class AddOpponentDialogModel
    {
        public OpponentModel Opponent { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddOpponentDialogModel()
        {
            Opponent = new OpponentModel();
        }
        
        /// <summary>
        /// Opens a folder picker to get the user to select the shared drive folder
        /// </summary>
        /// <returns>The folders path</returns>
        public async Task SelectSharedDrive()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            Opponent.SharedDir = folder.Path;
        }

        /// <summary>
        /// Adds the opponent to the list and updates the save file
        /// </summary>
        public void AddOpponent()
        {
            App.Opponents.Add(Opponent);
            //Assign its index so we know where to look to delete it
            Opponent.Index = App.Opponents.IndexOf(Opponent);
            StorageHelper.UpdateOpponentFile();
        }

    }
}
