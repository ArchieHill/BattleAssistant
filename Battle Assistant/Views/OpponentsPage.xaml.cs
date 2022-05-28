using Battle_Assistant.Helpers;
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
    /// Opponents page
    /// </summary>
    public sealed partial class OpponentsPage : Page
    {
        public OpponentsPageViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpponentsPage()
        {
            ViewModel = new OpponentsPageViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Add opponent click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOpponent_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddOpponent(this.Content.XamlRoot);
        }

        /// <summary>
        /// Delete opponent click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ViewModel.DeleteOpponent(int.Parse(btn.Tag.ToString()));
        }
    }
}
