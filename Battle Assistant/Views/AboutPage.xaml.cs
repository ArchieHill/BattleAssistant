using Microsoft.UI.Xaml.Controls;

namespace Battle_Assistant.Views
{
    /// <summary>
    /// An about page for the application
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public string Version
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
        }

        public AboutPage()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}
