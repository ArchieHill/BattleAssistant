// AddOpponentDialog.xaml.cs
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

using BattleAssistant.DialogModels;
using BattleAssistant.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BattleAssistant.Dialogs
{
    /// <summary>
    /// A content dialog to add an opponent
    /// </summary>
    public sealed partial class AddOpponentDialog : ContentDialog
    {
        private AddOpponentDialogModel DialogModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddOpponentDialog()
        {
            this.InitializeComponent();
            DialogModel = new AddOpponentDialogModel();
            DataContext = DialogModel;
        }

        /// <summary>
        /// Add Opponent Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddOpponent_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.AddOpponent();
        }

        /// <summary>
        /// Select Shared Drive Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SelectSharedDrive_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectSharedDrive();
            CheckInputs();
        }

        /// <summary>
        /// Opponents Name Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpponentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInputs();
        }

        /// <summary>
        /// Check inputs to enable primary button
        /// </summary>
        private void CheckInputs()
        {
            bool opponentNameUnique = true;
            foreach (OpponentModel opponent in App.Opponents)
            {
                if (opponent.Name == OpponentName.Text)
                {
                    DialogInfoBar.Severity = InfoBarSeverity.Error;
                    DialogInfoBar.Title = "Name not unique";
                    DialogInfoBar.Message = "The opponents name isn't unique";
                    DialogInfoBar.IsOpen = true;
                    opponentNameUnique = false;
                    break;
                }
            }

            IsPrimaryButtonEnabled = DialogModel.Opponent.SharedDir != null && OpponentName.Text != "" && opponentNameUnique;
        }
    }
}
