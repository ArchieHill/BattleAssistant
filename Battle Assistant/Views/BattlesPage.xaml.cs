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
    public sealed partial class BattlesPage : Page
    {
        public BattlesPageViewModel ViewModel { get; set; }

        public BattlesPage()
        {
            ViewModel = new BattlesPageViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }
        private void StartBattle_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartBattle(this.Content.XamlRoot);
        }
    }
}
