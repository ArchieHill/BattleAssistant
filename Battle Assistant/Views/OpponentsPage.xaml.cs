using Battle_Assistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            ViewModel.DeleteOpponent(int.Parse(btn.Tag.ToString()), this.Content.XamlRoot);
        }
    }
}
