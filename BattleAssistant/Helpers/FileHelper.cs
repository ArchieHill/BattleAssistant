// FileHelper.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BattleAssistant.Common;
using BattleAssistant.Models;
using Serilog;
using static PInvoke.User32;

namespace BattleAssistant.Helpers
{
    /// <summary>
    /// File Helper Methods
    /// </summary>
    public static class FileHelper
    {
        private const int FirstNumPosSubtractor = -3;
        private const int LastFileNamePosSubtractor = -4;
        private const int PreviousTurnSubtractor = -2;
        private const int PreviousFileSubtractor = -1;

        /// <summary>
        /// Copies the battle file to the incoming email folder through the following steps
        /// 1. Copies the new incoming battle file to the incoming email folder
        /// 2. Copies the new incoming battle file to the backup folder if needed
        /// 3. Deletes the previous incoming battle file in the incoming email
        /// 4. Deletes the new incoming battle file from the shared drive
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public static async Task CopyToIncomingEmailAsync(BattleModel battle)
        {
            try
            {
                //Copy the new incoming battle file to the incoming email folder
                while (IsFileLocked(new FileInfo(battle.BattleFile)))
                {
                    Log.Debug($"Waiting for file to unlock: {battle.BattleFile}");
                    await Task.Delay(500);
                }

                string fileInIncomingEmailPath = $@"{battle.Game.IncomingEmailFolder}\{Path.GetFileName(battle.BattleFile)}";
                File.Copy(battle.BattleFile, fileInIncomingEmailPath, true);
                Log.Information($"Copied {Path.GetFileName(battle.BattleFile)} to {fileInIncomingEmailPath}");

                //Backup the game file if the option is selected
                if (battle.Backup)
                {
                    string battleBackupFolder = $@"{SettingsHelper.GetBackupFolderPath()}\{battle.Name}";
                    Directory.CreateDirectory(battleBackupFolder);
                    File.Copy(battle.BattleFile, $@"{battleBackupFolder}\{Path.GetFileName(battle.BattleFile)}", true);
                    Log.Information($"Backed up file {Path.GetFileName(battle.BattleFile)} to {battleBackupFolder}");
                }

                //Delete new file in shared drive
                File.Delete(battle.BattleFile);
                Log.Information($"Deleted file {battle.BattleFile}");

                //Set the battle file to the new file location
                battle.BattleFile = fileInIncomingEmailPath;

                //Find the old file in the incoming email folder and delete it
                string oldFileInIncomingEmail = ConstructBattleFilePath(
                    battle.Game.IncomingEmailFolder,
                    $"~{battle.Name}",
                    GetFileNumber(battle.BattleFile) + PreviousTurnSubtractor);

                if (File.Exists(oldFileInIncomingEmail))
                {
                    while (IsFileLocked(new FileInfo(oldFileInIncomingEmail)))
                    {
                        Log.Debug($"Waiting for file to unlock: {oldFileInIncomingEmail}");
                        await Task.Delay(500);
                    }
                    File.Delete(oldFileInIncomingEmail);
                    Log.Information($"Deleted file {oldFileInIncomingEmail}");
                }

                battle.Status = Status.YourTurn;
                battle.LastAction = Actions.CopyToIncomingEmail;
                StorageHelper.UpdateBattleFile();

                if (SettingsHelper.GetFlashIcon())
                {
                    //Flash the icon in the task bar to notify the user the app has a turn for them
                    var flash = FLASHWINFO.Create();
                    flash.hwnd = App.Hwnd;
                    flash.uCount = SettingsHelper.GetFlashAmount();
                    flash.dwFlags = FlashWindowFlags.FLASHW_TRAY;
                    FlashWindowEx(ref flash);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

        }

        /// <summary>
        /// Copies the battle file to the shared drive folder through the following steps
        /// 1. Rename the incoming file to ~incoming file
        /// 2. Copy the new outgoing email file to the shared drive
        /// 3. Delete the new outgoing email file in the outgoing email file
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public static async Task CopyToSharedDriveAsync(BattleModel battle)
        {
            try
            {
                //Rename the file in the incoming email folder to show that that file is waiting on opponent
                string incomingFilePath = ConstructBattleFilePath(
                    battle.Game.IncomingEmailFolder,
                    battle.Name,
                    GetFileNumber(battle.BattleFile) + PreviousFileSubtractor);

                if (File.Exists(incomingFilePath))
                {
                    File.Move(incomingFilePath, $@"{battle.Game.IncomingEmailFolder}\~{Path.GetFileName(incomingFilePath)}");
                    File.Delete(battle.BattleFile);
                    Log.Information($"Renamed incoming email file to ~{Path.GetFileName(incomingFilePath)}");
                }

                //Copy the new outgoing email file to the shared drive
                while (IsFileLocked(new FileInfo(battle.BattleFile)))
                {
                    Log.Debug($"Waiting for file to unlock: {battle.BattleFile}");
                    await Task.Delay(500);
                }

                string fileInSharedDrivePath = $@"{battle.Opponent.SharedDir}\{Path.GetFileName(battle.BattleFile)}";
                File.Copy(battle.BattleFile, fileInSharedDrivePath, true);
                Log.Information($"Copied {Path.GetFileName(battle.BattleFile)} to {fileInSharedDrivePath}");

                //Delete the new outgoing email file from the outgoing email folder
                File.Delete(battle.BattleFile);
                Log.Information($"Deleted file {battle.BattleFile}");

                battle.BattleFile = fileInSharedDrivePath;
                battle.Status = Status.Waiting;
                battle.LastAction = Actions.CopyToSharedDrive;
                StorageHelper.UpdateBattleFile();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
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
            return fileName.Substring(0, fileName.Length + LastFileNamePosSubtractor);
        }

        /// <summary>
        /// Gets the files number in the name of the file
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <returns>The file number</returns>
        public static int GetFileNumber(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            return int.Parse(fileName.Substring(fileName.Length + FirstNumPosSubtractor));
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
            return $@"{folderPath}\{battleName} {number:D3}.ema";
        }

        /// <summary>
        /// Checks if the file is locked 
        /// </summary>
        /// <param name="file">The file that is being checked</param>
        /// <returns>If the file is locked or not</returns>
        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open,
                         FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null) { stream.Close(); }
            }

            //file is not locked
            return false;
        }
    }
}
