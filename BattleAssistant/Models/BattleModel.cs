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
using BattleAssistant.Interfaces;
using BattleAssistant.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BattleAssistant.Models
{
    /// <summary>
    /// The battle model
    /// </summary>
    public partial class BattleModel : MasterModel
    {
        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string lastAction;

        [ObservableProperty]
        private string battleFile;

        [ObservableProperty]
        private int currentFileNum;

        [ObservableProperty]
        private bool backup;

        public GameModel Game { get; set; }

        public OpponentModel Opponent { get; set; }

        partial void OnBattleFileChanged(string value)
        {
            Name = BattleFileHelper.GetFileDisplayName(value);
            CurrentFileNum = BattleFileHelper.GetFileNumber(value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BattleModel() : base()
        {
            CurrentFileNum = 1;
            Backup = false;
            Game = null;
            Opponent = null;
            Status = Common.Status.NoStatus;
            LastAction = Actions.NoLastAction;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="battleFile">The latest battlefile of the battle</param>
        /// <param name="game">The combat mission game for the battle</param>
        /// <param name="opponent">The opponent being played against</param>
        public BattleModel(string battleFile, GameModel game, OpponentModel opponent, bool backup)
        {
            BattleFile = battleFile;
            CurrentFileNum = BattleFileHelper.GetFileNumber(battleFile);
            Backup = backup;
            Game = game;
            Opponent = opponent;
            Status = Common.Status.NoStatus;
            LastAction = Actions.NoLastAction;
        }
    }
}
