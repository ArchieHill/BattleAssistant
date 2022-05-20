﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Battle_Assistant.Models;

namespace Battle_Assistant.DialogModels
{
    public class AddGameDialogModel
    {
        GameModel Game { get; set; }

        public AddGameDialogModel()
        {
            Game = new GameModel();
        }

        public async void SelectGameDir()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            Game.GameDir = await folderPicker.PickSingleFolderAsync();
        }

        public async void SelectIconFile()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".png");
            Game.GameIcon = await filePicker.PickSingleFileAsync();
        }
        public void AddGame()
        {
            App.Games.Add(Game);
        }
    }
}