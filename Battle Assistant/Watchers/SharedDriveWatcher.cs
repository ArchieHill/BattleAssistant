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
    /// Watches a shared drive folder
    /// </summary>
    public class SharedDriveWatcher : FolderWatcher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sharedDrivePath">The folders path</param>
        public SharedDriveWatcher(string sharedDrivePath) : base(sharedDrivePath)
        {

        }

        /// <summary>
        /// On file creation in the folder copies to incoming email folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The folder created</param>
        override
        protected void File_Created(object sender, FileSystemEventArgs e)
        {
            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Name == FileHelper.GetFileDisplayName(e.FullPath) && 
                    Path.GetFileName(battle.BattleFile) != e.Name)
                {
                    battle.BattleFile = e.FullPath;
                    FileHelper.CopyToIncomingEmail(battle);
                }
            }
        }
    }
}
