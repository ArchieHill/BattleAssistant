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

        protected abstract void File_Created(object sender, FileSystemEventArgs e);
    }
}
