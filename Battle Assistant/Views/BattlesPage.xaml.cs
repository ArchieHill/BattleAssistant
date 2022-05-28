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
    /// Battle page
    /// </summary>
    public sealed partial class BattlesPage : Page
    {
        public BattlesPageViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BattlesPage()
        {
            ViewModel = new BattlesPageViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Start battle click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBattle_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartBattle(this.Content.XamlRoot);
        }

        /// <summary>
        /// End battle click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndBattle_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ViewModel.EndBattle(int.Parse(btn.Tag.ToString()));
        }
    }
}
