using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        override
        protected async void File_CreatedTask(BattleModel battle, string newBattleFilePath)
        {
            battle.BattleFile = newBattleFilePath;
            while (IsFileLocked(new FileInfo(battle.BattleFile)))
            {
                Debug.WriteLine("Waiting for file to unlock");
                await Task.Delay(500);
            }
            FileHelper.CopyToIncomingEmail(battle);
        }
    }
}
