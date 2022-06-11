using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.DialogModels
{
    public class EndBattleConfirmationDialogModel
    {
        private BattleModel battle;

        public bool CleanUpFolders { get; set; }

        public EndBattleConfirmationDialogModel(BattleModel battle)
        {
            this.battle = battle;
        }

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
