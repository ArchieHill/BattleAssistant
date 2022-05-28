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

        override
        protected void File_CreatedTask(BattleModel battle, string newBattleFilePath)
        {
            battle.BattleFile = newBattleFilePath;
            FileHelper.CopyToIncomingEmail(battle);
        }
    }
}
