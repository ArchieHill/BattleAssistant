using Battle_Assistant.DialogModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Dialogs
{
    /// <summary>
    /// A content dialog to add a game
    /// </summary>
    public sealed partial class AddGameDialog : ContentDialog
    {
        private AddGameDialogModel DialogModel { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public AddGameDialog()
        {
            DialogModel = new AddGameDialogModel();
            DataContext = DialogModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// Add Game Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddGame_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.AddGame();
        }

        /// <summary>
        /// Select Game Directory Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SelectGameDir_Click(object sender, RoutedEventArgs e)
        {
            await DialogModel.SelectGameDir();
            CheckInputs();
        }

        /// <summary>
        /// Checks the inputs to enable the primary button
        /// </summary>
        private void CheckInputs()
        {
            if (DialogModel.Game.GameDir != null)
            {
                IsPrimaryButtonEnabled = true;
            }
        }
    }
}
