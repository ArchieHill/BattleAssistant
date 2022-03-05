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
        private CreateBattleViewModel viewModel;
        public CreateBattlePage()
        {
            var games = GamesPageViewModel.Instance.Games;

            viewModel = new CreateBattleViewModel();
            DataContext = viewModel;
            InitializeComponent();

            if (games.Count > 0)
            {
                viewModel.Games = games;
            }
            else
            {
                CreateBattleButton.IsEnabled = false;
                NoGameTB.Text = "No Games added";
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectGameFile();
        }

        private void SelectDriveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectDrive();
        }

        private void CreateBattle_Click(object sender, RoutedEventArgs e)
        {
            BattlesPageViewModel.Instance.Battles.Add(viewModel.CreateBattle());
        }
    }
}
