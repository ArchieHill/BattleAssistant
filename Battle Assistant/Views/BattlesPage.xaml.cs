using Battle_Assistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            ViewModel.EndBattle(int.Parse(btn.Tag.ToString()), this.Content.XamlRoot);
        }
    }
}
