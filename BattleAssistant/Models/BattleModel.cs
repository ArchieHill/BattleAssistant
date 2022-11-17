// BattleModel.cs
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

using BattleAssistant.Common;
using BattleAssistant.Helpers;

namespace BattleAssistant.Models
{
    /// <summary>
    /// The battle model
    /// </summary>
    public class BattleModel : MasterModel
    {
        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string lastAction;
        public string LastAction
        {
            get { return lastAction; }
            set
            {
                if (lastAction != value)
                {
                    lastAction = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string battleFile;
        public string BattleFile
        {
            get { return battleFile; }
            set
            {
                if (battleFile != value)
                {
                    battleFile = value;
                    SetVarsFromGameFile(battleFile);
                    NotifyPropertyChanged();
                }
            }
        }

        private int currentFileNum;
        public int CurrentFileNum
        {
            get { return currentFileNum; }
            set
            {
                if (currentFileNum != value)
                {
                    currentFileNum = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AutoClean { get; set; }

        public GameModel Game { get; set; }

        public OpponentModel Opponent { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BattleModel() : base()
        {
            Name = "";
            CurrentFileNum = 1;
            AutoClean = false;
            Game = null;
            Opponent = null;
            Status = Common.Status.NO_STATUS;
            LastAction = Actions.NO_LAST_ACTION;
        }

        /// <summary>
        /// Sets the name and file number from the battle file
        /// </summary>
        /// <param name="battleFile">The battle file path</param>
        public void SetVarsFromGameFile(string battleFile)
        {
            Name = FileHelper.GetFileDisplayName(battleFile);
            CurrentFileNum = FileHelper.GetFileNumber(battleFile);
        }
    }
}
