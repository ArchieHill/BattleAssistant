// SettingsPage.xaml.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Linq;
using BattleAssistant.Helpers;
using BattleAssistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BattleAssistant.Views
{
    /// <summary>
    /// Settings page
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsPageViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();
            viewModel = new SettingsPageViewModel(FlashAmountBox, AutoCreateOpponentSwitch, AutoCreateGameSwitch);
            DataContext = viewModel;
            Loaded += SettingsPage_Loaded;
        }

        /// <summary>
        /// Triggers when the settings page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SettingsPage_Loaded(object sender, RoutedEventArgs args)
        {
            string currentTheme = (this.XamlRoot.Content as Grid).RequestedTheme.ToString();
            (ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;

            viewModel.AutoSelectOpponent = SettingsHelper.GetAutoSelectOpponent();
            viewModel.AutoCreateOpponent = SettingsHelper.GetAutoCreateOpponent();
            viewModel.AutoSelectGame = SettingsHelper.GetAutoSelectGame();
            viewModel.AutoCreateGame = SettingsHelper.GetAutoCreateGame();

            viewModel.FlashIcon = SettingsHelper.GetFlashIcon();
            viewModel.FlashAmount = SettingsHelper.GetFlashAmount();
        }

        /// <summary>
        /// Sets a theme when the radio button is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnThemeModeChecked(object sender, RoutedEventArgs args)
        {
            string selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
            if (selectedTheme != null)
            {
                (App.MainWindow.Content as Grid).RequestedTheme = EnumHelper.GetEnum<ElementTheme>(selectedTheme);
                SettingsHelper.SaveTheme(selectedTheme);
            }
        }

        private async void SelectBackupFolder_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.SelectBackupFolder();
        }
    }
}
