﻿// EndBattleConfirmationDialogModel.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using BattleAssistant.Helpers;
using BattleAssistant.Models;
using Windows.Storage;

namespace BattleAssistant.DialogModels
{
    /// <summary>
    /// End Battle Confirmation Dialog Model
    /// </summary>
    public class EndBattleConfirmationDialogModel
    {
        private BattleModel battle;

        public bool CleanUpFolders { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="battle">The battle being ended</param>
        public EndBattleConfirmationDialogModel(BattleModel battle)
        {
            this.battle = battle;
        }

        /// <summary>
        /// Ends the battle
        /// </summary>
        public void EndBattle()
        {
            if (CleanUpFolders)
            {
                CleanFolder(battle.Game.IncomingEmailFolder);
                CleanFolder(battle.Game.OutgoingEmailFolder);
                CleanFolder(battle.Opponent.SharedDir);
            }
        }

        /// <summary>
        /// Deletes all the save files with the battles name
        /// </summary>
        /// <param name="folderPath">The folder being cleaned</param>
        private async void CleanFolder(string folderPath)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            foreach (StorageFile file in await folder.GetFilesAsync())
            {
                string fileDisplayName = FileHelper.GetFileDisplayName(file.Path);
                if (battle.Name == fileDisplayName || battle.Name == "~" + fileDisplayName)
                {
                    await file.DeleteAsync();
                }
            }
        }


    }
}
