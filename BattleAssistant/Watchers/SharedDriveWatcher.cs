// SharedDriveWatcher.cs
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

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BattleAssistant.Interfaces;
using BattleAssistant.Models;
using BattleAssistant.Services;
using Serilog;

namespace BattleAssistant.Watchers
{
    /// <summary>
    /// Watches a shared drive folder
    /// </summary>
    public class SharedDriveWatcher : FolderWatcher
    {
        public SharedDriveWatcher(string folderPath) : base(folderPath)
        {
        }

        protected override async void File_CreatedTask(BattleModel battle, string newBattleFilePath)
        {
            //Sometimes multiple events will fire for the same task in quick succession, if the file is already the battle file then we know its already been processed
            if (Path.GetFileName(battle.BattleFile) == Path.GetFileName(newBattleFilePath))
            {
                Log.Debug("File already processed");
                return;
            }

            battle.BattleFile = newBattleFilePath;
            await FileService.CopyToIncomingEmailAsync(battle);
        }
    }
}
