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
using CommunityToolkit.Mvvm.ComponentModel;

namespace BattleAssistant.Models
{
    /// <summary>
    /// The game model
    /// </summary>
    public partial class GameModel : MasterModel, IDisposable
    {
        private OutGoingEmailFolderWatcher oGEFWatcher;

        [ObservableProperty]
        private string gameDir;

        [ObservableProperty]
        private string incomingEmailFolder;

        [ObservableProperty]
        private string outgoingEmailFolder;

        partial void OnGameDirChanged(string value)
        {
            Name = Path.GetFileNameWithoutExtension(value);

            //Set email folders
            IncomingEmailFolder = $@"{GameDir}\Game Files\Incoming Email";
            OutgoingEmailFolder = $@"{GameDir}\Game Files\Outgoing Email";
        }
        
        partial void OnIncomingEmailFolderChanged(string value)
        {
            oGEFWatcher = new(value);
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameDir">The game directory</param>
        public GameModel(string gameDir) : base()
        {
            GameDir = gameDir;
        }
        
        public void Dispose()
        {
            oGEFWatcher.Dispose();
        }
    }
}
