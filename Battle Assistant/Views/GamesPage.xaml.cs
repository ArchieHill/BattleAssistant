using Battle_Assistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Views
{
    /// <summary>
    /// The games page
    /// </summary>
    public sealed partial class GamesPage : Page
    {
        public GamesPageViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GamesPage()
        {
            ViewModel = new GamesPageViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Add game click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddGame(this.Content.XamlRoot);
        }

        /// <summary>
        /// Delete game click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ViewModel.DeleteGame(int.Parse(btn.Tag.ToString()), this.Content.XamlRoot);
        }
    }
}
