using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using System.IO;

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
