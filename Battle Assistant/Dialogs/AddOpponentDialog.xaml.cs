using Battle_Assistant.DialogModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Dialogs
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
            DialogModel = new AddOpponentDialogModel();
            DataContext = DialogModel;
            this.InitializeComponent();
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
            if (DialogModel.Opponent.SharedDir != null && DialogModel.Opponent.Name != "")
            {
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
