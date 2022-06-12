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
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            if(DialogModel.Battle.BattleFile != null && GameAndOpponentExist)
            {
                IsPrimaryButtonEnabled = true;
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
