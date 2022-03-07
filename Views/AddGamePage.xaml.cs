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
    public sealed partial class AddGamePage : Page
    {
        private AddGameViewModel viewModel;
        public AddGamePage()
        {
            viewModel = new AddGameViewModel();
            DataContext = viewModel;
            InitializeComponent();    
        }

        private void SelectIconButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectIcon();
        }

        private void SelectGameFilesDirButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectGameFilesDir();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            GamesPageViewModel.Instance.Games.Add(viewModel.AddGame());
            var nav = (Application.Current as App).Navigation;
            var gamesItem = nav.GetNavigationViewItems(typeof(GamesPage)).First();
            nav.SetCurrentNavigationViewItem(gamesItem);
        }
    }
}
