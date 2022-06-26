// StartBattleDialog.xaml.cs
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

using Battle_Assistant.DialogModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Dialogs
{
    /// <summary>
    /// A content dialog to add a battle
    /// </summary>
    public sealed partial class StartBattleDialog : ContentDialog
    {
        private StartBattleDialogModel DialogModel { get; set; }

        private bool GameAndOpponentExist;

        /// <summary>
        /// Constructor
        /// </summary>
        public StartBattleDialog()
        {
            this.InitializeComponent();
            DialogModel = new StartBattleDialogModel(DialogInfoBar);
            DataContext = DialogModel;
            Loaded += StartBattleDialog_Loaded;
            GameAndOpponentExist = false;
        }

        /// <summary>
        /// Page Loaded Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBattleDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //If at least one game and opponent exist then allow a battle to be started
            if (DialogModel.Games.Count > 0 && DialogModel.Opponents.Count > 0)
            {
                GameAndOpponentExist = true;
            }
        }

        /// <summary>
        /// Start Battle Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StartBattle_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.StartBattle();
        }

        /// <summary>
        /// Select Battle File Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectBattleFile();
            CheckInputs();
        }

        /// <summary>
        /// Checks the inputs and enables the primary button
        /// </summary>
        private void CheckInputs()
        {
            if (DialogModel.Battle.BattleFile != null && GameAndOpponentExist)
            {
                IsPrimaryButtonEnabled = true;
            }
            else
            {
                IsPrimaryButtonEnabled = false;
            }
        }

        private void AutoClean_Checked(object sender, RoutedEventArgs e)
        {
            DialogModel.Battle.AutoClean = true;
        }

        private void AutoClean_Unchecked(object sender, RoutedEventArgs e)
        {
            DialogModel.Battle.AutoClean = false;
        }
    }
}
