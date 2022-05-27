﻿using System;
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
    public class AddOpponentDialogModel
    {
        public OpponentModel Opponent { get; set; }

        public AddOpponentDialogModel()
        {
            Opponent = new OpponentModel();
        }
        
        public async Task SelectSharedDrive()
        {
            FolderPicker folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.Hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            Opponent.SharedDir = folder.Path;
        }

        public void AddOpponent()
        {
            App.Opponents.Add(Opponent);
            //Assign its index so we know where to look to delete it
            Opponent.Index = App.Opponents.IndexOf(Opponent);
            StorageHelper.UpdateOpponentFile();
        }

    }
}
