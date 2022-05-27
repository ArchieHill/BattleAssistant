using Battle_Assistant.Helpers;
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
        public OutGoingEmailFolderWatcher(string oGEFFolderPath) : base(oGEFFolderPath)
        {

        }

        override
        protected void File_Created(object sender, FileSystemEventArgs e)
        {
            foreach(BattleModel battle in App.Battles)
            {
                if(battle.Name == FileHelper.GetFileDisplayName(e.FullPath) && 
                    Path.GetFileName(battle.BattleFile) != e.Name)
                {
                    battle.BattleFile = e.FullPath;
                    FileHelper.CopyToSharedDrive(battle);
                }
            }
        }
    }
}
