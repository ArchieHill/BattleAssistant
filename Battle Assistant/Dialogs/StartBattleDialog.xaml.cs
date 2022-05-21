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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartBattleDialog : ContentDialog
    {
        private StartBattleDialogModel DialogModel { get; } = new StartBattleDialogModel();

        private bool GameAndOpponentExist;
        public StartBattleDialog()
        {
            this.InitializeComponent();
            DataContext = DialogModel;
            Loaded += StartBattleDialog_Loaded;
            GameAndOpponentExist = false;
        }

        private void StartBattleDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //If at least one game and opponent exist then allow a battle to be started
            if (DialogModel.Games.Count > 0 && DialogModel.Opponents.Count > 0)
            {
                GameAndOpponentExist = true;
            }
        }

        private void StartBattle_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.StartBattle();
        }

        private async void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectBattleFile();
            CheckInputs();
        }

        private void CheckInputs()
        {
            if(DialogModel.Battle.InitialGameFile != null && GameAndOpponentExist)
            {
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
