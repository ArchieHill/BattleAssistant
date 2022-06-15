// OutGoingEmailFolderWatcher.cs
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

using System.IO;
using System.Threading.Tasks;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;

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
            //Sometimes multiple events will fire for the same task in quick succession, if the file is already the battle file then we know its already been processed
            if(Path.GetFileName(battle.BattleFile) == Path.GetFileName(newBattleFilePath))
            {
                return;
            }
            //Rename the file in the incoming email folder to show that that file is waiting on opponent
            File.Move(battle.BattleFile, battle.Game.IncomingEmailFolder + "\\~" + Path.GetFileName(battle.BattleFile));
            File.Delete(battle.BattleFile);

            battle.BattleFile = newBattleFilePath;
            while (IsFileLocked(new FileInfo(battle.BattleFile)))
            {
                await Task.Delay(500);
            }

            FileHelper.CopyToSharedDrive(battle);
        }
    }
}
