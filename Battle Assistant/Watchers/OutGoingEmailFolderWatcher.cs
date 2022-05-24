﻿using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Watchers
{
    public class OutGoingEmailFolderWatcher : FolderWatcher
    {
        public OutGoingEmailFolderWatcher(StorageFolder oGEFFolder) : base(oGEFFolder)
        {

        }

        override
        protected async void File_Created(object sender, FileSystemEventArgs e)
        {
            foreach(BattleModel battle in App.Battles)
            {
                if(battle.Name == e.Name.Substring(0, -7) && battle.BattleFile.Name != e.Name)
                {
                    battle.BattleFile = await StorageFile.GetFileFromPathAsync(e.FullPath);
                    FileHelper.CopyToSharedDrive(battle);
                }
            }
        }
    }
}
