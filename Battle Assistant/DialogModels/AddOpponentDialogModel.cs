﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Assistant.Models;
using Windows.Storage.Pickers;

namespace Battle_Assistant.DialogModels
{
    
    public class AddOpponentDialogModel
    {
        OpponentModel Opponent { get; set; }

        public AddOpponentDialogModel()
        {
            Opponent = new OpponentModel();
        }
        
        public async void SelectSharedDrive()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            Opponent.SharedDir = await folderPicker.PickSingleFolderAsync();
        }
        public void AddOpponent()
        {
            App.Opponents.Add(Opponent);
        }
    }
}