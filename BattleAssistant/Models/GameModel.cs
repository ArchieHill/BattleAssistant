// GameModel.cs
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
using BattleAssistant.Watchers;

namespace BattleAssistant.Models
{
    /// <summary>
    /// The game model
    /// </summary>
    public class GameModel : MasterModel, IDisposable
    {
        private OutGoingEmailFolderWatcher oGEFWatcher;

        private string gameDir;
        public string GameDir
        {
            get { return gameDir; }
            set
            {
                if (gameDir != value)
                {
                    gameDir = value;
                    Name = Path.GetFileNameWithoutExtension(value);
                    SetEmailFolders();
                    NotifyPropertyChanged();
                }
            }
        }

        private string incomingEmailFolder;
        public string IncomingEmailFolder
        {
            get { return incomingEmailFolder; }
            set
            {
                if (incomingEmailFolder != value)
                {
                    incomingEmailFolder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string outgoingEmailFolder;
        public string OutgoingEmailFolder
        {
            get { return outgoingEmailFolder; }
            set
            {
                if (outgoingEmailFolder != value)
                {
                    outgoingEmailFolder = value;
                    oGEFWatcher = new OutGoingEmailFolderWatcher(outgoingEmailFolder);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameModel() : base()
        {
            GameDir = null;
            IncomingEmailFolder = null;
            OutgoingEmailFolder = null;
        }

        public GameModel(string gameDir) : base()
        {
            GameDir = gameDir;
        }

        /// <summary>
        /// Sets the email folders from the game directory folder
        /// </summary>
        public void SetEmailFolders()
        {
            IncomingEmailFolder = $@"{GameDir}\Game Files\Incoming Email";
            OutgoingEmailFolder = $@"{GameDir}\Game Files\Outgoing Email";
        }

        public void Dispose()
        {
            oGEFWatcher.Dispose();
        }
    }
}
