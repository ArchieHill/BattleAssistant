﻿// OpponentModel.cs
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
using BattleAssistant.Watchers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BattleAssistant.Models
{
    /// <summary>
    /// Opponents model
    /// </summary>
    public partial class OpponentModel : MasterModel, IDisposable
    {
        private SharedDriveWatcher sDWatcher;

        [ObservableProperty]
        private string sharedDir;

        partial void OnSharedDirChanged(string value)
        {
            sDWatcher = new(value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpponentModel() : base()
        {
            SharedDir = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The opponent's name</param>
        /// <param name="sharedDir">The shared directory with the opponent</param>
        public OpponentModel(string name, string sharedDir) : base(name)
        {
            SharedDir = sharedDir;
        }

        public void Dispose()
        {
            sDWatcher.Dispose();
        }
    }
}
