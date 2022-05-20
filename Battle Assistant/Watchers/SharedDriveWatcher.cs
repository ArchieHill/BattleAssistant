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
    public class SharedDriveWatcher
    {
        public FileSystemWatcher Watcher { get; set; }

        public SharedDriveWatcher(StorageFolder sharedDrive)
        {
            Watcher = new FileSystemWatcher(sharedDrive.Path);
            Watcher.Filter = "*.ema";
            Watcher.Created += File_Created;
        }

        private void File_Created(object sender, FileSystemEventArgs e)
        {
            foreach (BattleModel battle in App.Battles)
            {
                if (battle.Name == e.Name.Substring(0, -4))
                {
                    File.Copy(e.FullPath, battle.Game.IncomingEmailFolder.Path + e.Name);
                }
            }
        }
    }
}
