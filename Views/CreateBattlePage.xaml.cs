using Battle_Assistant.Models;
using Battle_Assistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateBattlePage : Page
    {
        public CreateBattleViewModel ViewModel { get; set; } = new CreateBattleViewModel();
        public CreateBattlePage()
        {
            DataContext = ViewModel;
            InitializeComponent();

            if (App.Games.Count > 0)
            {
                ViewModel.Games = App.Games;
            }
            else
            {
                CreateBattleButton.IsEnabled = false;
                NoGameTB.Text = "No Games added";
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectGameFile();
        }

        private void SelectDriveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectDrive();
        }

        private void CreateBattle_Click(object sender, RoutedEventArgs e)
        {
            App.Battles.Add(ViewModel.CreateBattle());
            var nav = (Application.Current as App).Navigation;
            var battlesItem = nav.GetNavigationViewItems(typeof(BattlesPage)).First();
            nav.SetCurrentNavigationViewItem(battlesItem);
        }
    }
}
