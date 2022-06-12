using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using Windows.Storage;

namespace Battle_Assistant.DialogModels
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
            if(CleanUpFolders)
            {
                CleanFolder(battle.Game.IncomingEmailFolder);
                CleanFolder(battle.Game.OutgoingEmailFolder);
                CleanFolder(battle.Opponent.SharedDir);
            }
        }

        private async void CleanFolder(string folderPath)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            foreach(StorageFile file in await folder.GetFilesAsync())
            {
                if(battle.Name == FileHelper.GetFileDisplayName(file.Path))
                {
                    await file.DeleteAsync();
                }
            }
        }


    }
}
