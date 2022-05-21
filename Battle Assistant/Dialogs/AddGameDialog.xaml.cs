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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddGameDialog : ContentDialog
    {
        private AddGameDialogModel DialogModel { get; set; } 
        public AddGameDialog()
        {
            DialogModel = new AddGameDialogModel();
            DataContext = DialogModel;
            this.InitializeComponent();
        }

        private void AddGame_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.AddGame();
        }

        private async void SelectGameDir_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectGameDir();
            CheckInputs();
        }

        private void SelectIconFile_Click(object sender, RoutedEventArgs e)
        {
            DialogModel.SelectIconFile();
        }

        private void CheckInputs()
        {
            if (DialogModel.Game.GameDir != null)
            {
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
