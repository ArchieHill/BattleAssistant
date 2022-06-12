using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System.IO;
using System.Threading.Tasks;

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
        /// The task called when a file is created and is part of a battle
        /// </summary>
        /// <param name="battle">The battle the file is a part of</param>
        /// <param name="newBattleFilePath">The file path of the new battle file</param>
        override
        protected async void File_CreatedTask(BattleModel battle, string newBattleFilePath)
        {
            //Rename the file in the incoming email folder to show that that file is waiting on opponent
            File.Move(battle.BattleFile, battle.Game.IncomingEmailFolder + "\\~" + Path.GetFileName(battle.BattleFile));
            File.Delete(battle.BattleFile);

            battle.BattleFile = newBattleFilePath;
            while(IsFileLocked(new FileInfo(battle.BattleFile)))
            {
                await Task.Delay(500);
            }
            
            FileHelper.CopyToSharedDrive(battle);
        }
    }
}
