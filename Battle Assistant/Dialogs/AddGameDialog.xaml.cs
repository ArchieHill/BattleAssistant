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
    /// A content dialog to add a game
    /// </summary>
    public sealed partial class AddGameDialog : ContentDialog
    {
        private AddGameDialogModel DialogModel { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public AddGameDialog()
        {
            DialogModel = new AddGameDialogModel();
            DataContext = DialogModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Add Game Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddGame_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.AddGame();
        }

        /// <summary>
        /// Select Game Directory Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SelectGameDir_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectGameDir();
            CheckInputs();
        }

        /// <summary>
        /// Select Icon File Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectIconFile_Click(object sender, RoutedEventArgs e)
        {
            DialogModel.SelectIconFile();
        }

        /// <summary>
        /// Checks the inputs to enable the primary button
        /// </summary>
        private void CheckInputs()
        {
            if (DialogModel.Game.GameDir != null)
            {
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
