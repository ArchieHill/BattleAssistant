// FolderWatcher.cs
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
using System.IO;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;

namespace Battle_Assistant.Watchers
{
    /// <summary>
    /// Template class for folder watchers
    /// </summary>
    public abstract class FolderWatcher : IDisposable
    {
        protected FileSystemWatcher Watcher { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath">The folder path</param>
        public FolderWatcher(string folderPath)
        {

            Watcher = new FileSystemWatcher();
            Watcher.Path = folderPath;
            Watcher.Filter = "*.ema";
            Watcher.Created += new FileSystemEventHandler(File_Created);
            Watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Finds the battle the new file is part of and actions on the file with File_CreatedTask
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The file object</param>
        protected void File_Created(object sender, FileSystemEventArgs e)
        {
            string createdFile = e.FullPath.Replace("-TEMP", "");
            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Name == FileHelper.GetFileDisplayName(createdFile) &&
                    Path.GetFileName(battle.BattleFile) != Path.GetFileName(createdFile))
                {
                    //This if statement is used to allow the file watcher thread to access the UI thread
                    if (App.MainWindow.DispatcherQueue.HasThreadAccess)
                    {
                        File_CreatedTask(battle, createdFile);
                    }
                    else
                    {
                        bool isQueued = App.MainWindow.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                        () => File_CreatedTask(battle, createdFile));
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the file is locked 
        /// </summary>
        /// <param name="file">The file that is being checked</param>
        /// <returns>If the file is locked or not</returns>
        protected static bool IsFileLocked(FileInfo file)
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
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// The task called when a file is created and is part of a battle
        /// </summary>
        /// <param name="battle">The battle the file is a part of</param>
        /// <param name="newBattleFilePath">The file path of the new battle file</param>
        protected abstract void File_CreatedTask(BattleModel battle, string newBattleFilePath);

        public void Dispose()
        {
            Watcher.Dispose();
        }
    }
}
