using Battle_Assistant.DialogModels;
using Battle_Assistant.Models;
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
    /// A end battle confirmation dialog
    /// </summary>
    public sealed partial class EndBattleConfirmationDialog : ContentDialog
    {
        private EndBattleConfirmationDialogModel DialogModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndBattleConfirmationDialog(BattleModel battle)
        {
            DialogModel = new EndBattleConfirmationDialogModel(battle);
            DataContext = DialogModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Clenup checkbox checked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanUp_Checked(object sender, RoutedEventArgs e)
        {
            DialogModel.CleanUpFolders = true;
        }

        /// <summary>
        /// Cleanup checkbox uncheck event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanUp_Unchecked(object sender, RoutedEventArgs e)
        {
            DialogModel.CleanUpFolders = false;
        }

        /// <summary>
        /// End battle click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EndBattle_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.EndBattle();
        }
    }
}
