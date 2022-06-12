using Battle_Assistant.Common;
using Battle_Assistant.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Battle_Assistant.Helpers
{
    /// <summary>
    /// File Helper Methods
    /// </summary>
    public static class FileHelper
    {
        private const int FIRST_NUM_POS_SUBTRACTOR = -3;

        private const int LAST_FILE_NAME_POS_SUBTRACTOR = -4;

        /// <summary>
        /// Copies the battle file to the incoming email folder
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public static void CopyToIncomingEmail(BattleModel battle)
        {
            string fileInIncomingEmailPath = battle.Game.IncomingEmailFolder + "\\" + Path.GetFileName(battle.BattleFile);
            try
            {
                File.Copy(battle.BattleFile, fileInIncomingEmailPath, true);
                if (battle.AutoClean)
                {
                    //Find the old file in the incoming email folder and delete it
                    string oldFileInIncomingEmail = ConstructBattleFilePath(battle.Game.IncomingEmailFolder, "~" + battle.Name, GetFileNumber(battle.BattleFile) - 2);
                    if (File.Exists(oldFileInIncomingEmail))
                    {
                        File.Delete(oldFileInIncomingEmail);
                    }
                }
                battle.BattleFile = fileInIncomingEmailPath;

                battle.Status = Status.YOUR_TURN;
                battle.LastAction = Actions.COPY_TO_INCOMING_EMAIL;
                StorageHelper.UpdateBattleFile();
            }
            catch (Exception e)
            {
                Debug.WriteLine("File failed to copy: " + e.Message);
            }
            
        }

        /// <summary>
        /// Copies the battle file to the shared drive folder
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public static void CopyToSharedDrive(BattleModel battle)
        {
            string fileInSharedDrivePath = battle.Opponent.SharedDir + "\\" + Path.GetFileName(battle.BattleFile);
            try
            {
                File.Copy(battle.BattleFile, fileInSharedDrivePath, true);
                if (battle.AutoClean)
                {
                    //Find the old battle file in the outgoing email folder and delete it
                    string oldFileInOutgoingEmail = ConstructBattleFilePath(battle.Game.OutgoingEmailFolder, battle.Name, GetFileNumber(battle.BattleFile) - 2);
                    if (File.Exists(oldFileInOutgoingEmail))
                    {
                        File.Delete(oldFileInOutgoingEmail);
                    }

                    //Find the old battle file in the shared drive and delete it
                    string oldFileInSharedDrive = ConstructBattleFilePath(battle.Opponent.SharedDir, battle.Name, GetFileNumber(battle.BattleFile) - 2);
                    if (File.Exists(oldFileInSharedDrive))
                    {
                        File.Delete(oldFileInSharedDrive);
                    }
                }
                battle.BattleFile = fileInSharedDrivePath;

                battle.Status = Status.WAITING;
                battle.LastAction = Actions.COPY_TO_SHAREDDRIVE;
                StorageHelper.UpdateBattleFile();
            }
            catch (Exception e)
            {
                Debug.WriteLine("File failed to copy: " + e.Message);
            }
            
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
            return int.Parse(fileName.Substring(fileName.Length + FIRST_NUM_POS_SUBTRACTOR));
        }

        /// <summary>
        /// Checks if the file has its three numbers at the end
        /// </summary>
        /// <param name="path">The file path being checked</param>
        /// <returns>If the file is valid</returns>
        public static bool CheckFileIsValid(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileNumbers = fileName.Substring(fileName.Length + FIRST_NUM_POS_SUBTRACTOR);
            return Regex.IsMatch(fileNumbers, "/d/d/d");

        }

        /// <summary>
        /// Constructs a battle file path
        /// </summary>
        /// <param name="folderPath">The folder the battle file will be in</param>
        /// <param name="battleName">The battle files name</param>
        /// <param name="number">The battle files number</param>
        /// <returns>The full path of the constructed battle file</returns>
        public static string ConstructBattleFilePath(string folderPath, string battleName, int number)
        {
            return folderPath + "\\" + battleName + " " + number.ToString("D3") + ".ema";
        }
    }
}
