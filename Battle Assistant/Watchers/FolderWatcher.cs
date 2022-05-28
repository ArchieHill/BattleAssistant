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
    /// Template class for folder watchers
    /// </summary>
    public abstract class FolderWatcher
    {
        public FileSystemWatcher Watcher { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath">The folder path</param>
        public FolderWatcher(string folderPath)
        {

            Watcher = new FileSystemWatcher(folderPath);
            Watcher.Filter = "*.ema";
            Watcher.Created += File_Created;
            Watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Finds the battle the new file is part of and actions on the file with File_CreatedTask
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The file object</param>
        protected void File_Created(object sender, FileSystemEventArgs e)
        {
            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Name == FileHelper.GetFileDisplayName(e.FullPath) &&
                    Path.GetFileName(battle.BattleFile) != e.Name)
                {
                    //This if statement is used to allow the file watcher thread to access the UI thread
                    if (App.MainWindow.DispatcherQueue.HasThreadAccess)
                    {
                        File_CreatedTask(battle, e.FullPath);
                    }
                    else
                    {
                        bool isQueued = App.MainWindow.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                        () => File_CreatedTask(battle, e.FullPath));
                    }
                }
            }
        }

        /// <summary>
        /// The task called when a file is created and is part of a battle
        /// </summary>
        /// <param name="battle">The battle the file is a part of</param>
        /// <param name="newBattleFilePath">The file path of the new battle file</param>
        protected abstract void File_CreatedTask(BattleModel battle, string newBattleFilePath);
    }
}
