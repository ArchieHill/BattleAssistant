using Battle_Assistant.Common;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Helpers
{
    public static class FileHelper
    {
        private const int FIRST_NUM_POS_SUBTRACTOR = -3;

        private const int LAST_FILE_NAME_POS_SUBTRACTOR = -4;

        public static void CopyToIncomingEmail(BattleModel battle)
        {
            File.Copy(battle.BattleFile, battle.Game.IncomingEmailFolder + "\\" + Path.GetFileName(battle.BattleFile), true);
            battle.Status = Status.YOUR_TURN;
            battle.LastAction = Actions.COPY_TO_INCOMING_EMAIL;
        }

        public static void CopyToSharedDrive(BattleModel battle)
        {
            File.Copy(battle.BattleFile, battle.Opponent.SharedDir + "\\" + Path.GetFileName(battle.BattleFile), true);
            battle.Status = Status.WAITING;
            battle.LastAction = Actions.COPY_TO_SHAREDDRIVE;
        }

        /// <summary>
        /// Gets the files name without the file number at the end
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <returns></returns>
        public static string GetFileDisplayName(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            return fileName.Substring(0, fileName.Length + LAST_FILE_NAME_POS_SUBTRACTOR);
        }

        /// <summary>
        /// Gets the files number in the name of the file
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <returns>The file number</returns>
        public static int GetFileNumber(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            return int.Parse(fileName.Substring(0, fileName.Length + FIRST_NUM_POS_SUBTRACTOR));
        }
    }
}
