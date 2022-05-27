using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Watchers
{
    public abstract class FolderWatcher
    {
        public FileSystemWatcher Watcher { get; set; }

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
