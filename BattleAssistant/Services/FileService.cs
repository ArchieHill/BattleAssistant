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
using BattleAssistant.Helpers;
using BattleAssistant.Interfaces;
using BattleAssistant.Models;
using Serilog;
using static PInvoke.User32;

namespace BattleAssistant.Services
{
    /// <summary>
    /// File Helper Methods
    /// </summary>
    public class FileService : IFileService
    {

        private const int PreviousTurnSubtractor = -2;
        private const int PreviousFileSubtractor = -1;

        private readonly ILogger Logger;
        private readonly ISettingsService SettingsService;
        private readonly IStorageService StorageService;

        public FileService(ILogger logger, ISettingsService settingsService, IStorageService storageService)
        {
            Logger = logger;
            SettingsService = settingsService;
            StorageService = storageService;
        }

        /// <summary>
        /// Copies the battle file to the incoming email folder through the following steps
        /// 1. Copies the new incoming battle file to the incoming email folder
        /// 2. Copies the new incoming battle file to the backup folder if needed
        /// 3. Deletes the previous incoming battle file in the incoming email
        /// 4. Deletes the new incoming battle file from the shared drive
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public async Task CopyToIncomingEmailAsync(BattleModel battle)
        {
            try
            {
                //Copy the new incoming battle file to the incoming email folder
                while (FileHelper.FileIsLocked(new FileInfo(battle.BattleFile)))
                {
                    Logger.Debug($"Waiting for file to unlock: {battle.BattleFile}");
                    await Task.Delay(500);
                }

                var fileInIncomingEmailPath = $@"{battle.Game.IncomingEmailFolder}\{Path.GetFileName(battle.BattleFile)}";
                File.Copy(battle.BattleFile, fileInIncomingEmailPath, true);
                Logger.Information($"Copied {Path.GetFileName(battle.BattleFile)} to {fileInIncomingEmailPath}");

                //Backup the game file if the option is selected
                if (battle.Backup)
                {
                    var battleBackupFolder = $@"{SettingsService.GetBackupFolderPath()}\{battle.Name}";
                    Directory.CreateDirectory(battleBackupFolder);
                    File.Copy(battle.BattleFile, $@"{battleBackupFolder}\{Path.GetFileName(battle.BattleFile)}", true);
                    Logger.Information($"Backed up file {Path.GetFileName(battle.BattleFile)} to {battleBackupFolder}");
                }

                //Delete new file in shared drive
                File.Delete(battle.BattleFile);
                Logger.Information($"Deleted file {battle.BattleFile}");

                //Set the battle file to the new file location
                battle.BattleFile = fileInIncomingEmailPath;

                //Find the old file in the incoming email folder and delete it
                var oldFileInIncomingEmail = BattleFileHelper.ConstructFilePath(
                    battle.Game.IncomingEmailFolder,
                    $"~{battle.Name}",
                    BattleFileHelper.GetFileNumber(battle.BattleFile) + PreviousTurnSubtractor);

                if (File.Exists(oldFileInIncomingEmail))
                {
                    while (FileHelper.FileIsLocked(new FileInfo(oldFileInIncomingEmail)))
                    {
                        Logger.Debug($"Waiting for file to unlock: {oldFileInIncomingEmail}");
                        await Task.Delay(500);
                    }
                    File.Delete(oldFileInIncomingEmail);
                    Logger.Information($"Deleted file {oldFileInIncomingEmail}");
                }

                battle.Status = Status.YourTurn;
                battle.LastAction = Actions.CopyToIncomingEmail;
                await StorageService.UpdateBattleFile();

                if (SettingsService.GetFlashIcon())
                {
                    //Flash the icon in the task bar to notify the user the app has a turn for them
                    var flash = FLASHWINFO.Create();
                    flash.hwnd = App.Hwnd;
                    flash.uCount = SettingsService.GetFlashAmount();
                    flash.dwFlags = FlashWindowFlags.FLASHW_TRAY;
                    FlashWindowEx(ref flash);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }

        }

        /// <summary>
        /// Copies the battle file to the shared drive folder through the following steps
        /// 1. Rename the incoming file to ~incoming file
        /// 2. Copy the new outgoing email file to the shared drive
        /// 3. Delete the new outgoing email file in the outgoing email file
        /// </summary>
        /// <param name="battle">The battle that has its file being copied</param>
        public async Task CopyToSharedDriveAsync(BattleModel battle)
        {
            try
            {
                //Rename the file in the incoming email folder to show that that file is waiting on opponent
                var incomingFilePath = BattleFileHelper.ConstructFilePath(
                    battle.Game.IncomingEmailFolder,
                    battle.Name,
                    BattleFileHelper.GetFileNumber(battle.BattleFile) + PreviousFileSubtractor);

                if (File.Exists(incomingFilePath))
                {
                    File.Move(incomingFilePath, $@"{battle.Game.IncomingEmailFolder}\~{Path.GetFileName(incomingFilePath)}");
                    File.Delete(battle.BattleFile);
                    Logger.Information($"Renamed incoming email file to ~{Path.GetFileName(incomingFilePath)}");
                }

                //Copy the new outgoing email file to the shared drive
                while (FileHelper.FileIsLocked(new FileInfo(battle.BattleFile)))
                {
                    Logger.Debug($"Waiting for file to unlock: {battle.BattleFile}");
                    await Task.Delay(500);
                }

                var fileInSharedDrivePath = $@"{battle.Opponent.SharedDir}\{Path.GetFileName(battle.BattleFile)}";
                File.Copy(battle.BattleFile, fileInSharedDrivePath, true);
                Logger.Information($"Copied {Path.GetFileName(battle.BattleFile)} to {fileInSharedDrivePath}");

                //Delete the new outgoing email file from the outgoing email folder
                File.Delete(battle.BattleFile);
                Logger.Information($"Deleted file {battle.BattleFile}");

                battle.BattleFile = fileInSharedDrivePath;
                battle.Status = Status.Waiting;
                battle.LastAction = Actions.CopyToSharedDrive;
                await StorageService.UpdateBattleFile();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }
        }
    }
}
