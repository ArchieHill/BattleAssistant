using Battle_Assistant.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Views
{
    /// <summary>
    /// Settings page
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = (this.XamlRoot.Content as Grid).RequestedTheme.ToString();
            (ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;
        }

        private void OnThemeModeChecked(object sender, RoutedEventArgs args)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
            if(selectedTheme != null)
            {
                (App.MainWindow.Content as Grid).RequestedTheme = EnumHelper.GetEnum<ElementTheme>(selectedTheme);
                SettingsHelper.SaveTheme(selectedTheme);
            }
        }
    }
}
