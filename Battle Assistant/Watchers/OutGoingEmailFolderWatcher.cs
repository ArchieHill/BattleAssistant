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
    /// <summary>
    /// Watches the outgoing email folders
    /// </summary>
    public class OutGoingEmailFolderWatcher : FolderWatcher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oGEFFolderPath"></param>
        public OutGoingEmailFolderWatcher(string oGEFFolderPath) : base(oGEFFolderPath)
        {

        }

        /// <summary>
        /// On a file creation in the folder copies to shared drive folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The folder thats been created</param>
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
