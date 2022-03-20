using Battle_Assistant.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Shell Window;

        public static IntPtr Hwnd;
        public static ObservableCollection<BattleModel> Battles { get; set; }

        public static ObservableCollection<GameModel> Games { get; set; }

        public INavigation Navigation => m_window;

        private const string GAMES_FILE_NAME = "games.json";

        private const string BATTLES_FILE_NAME = "battles.json";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            Battles = new ObservableCollection<BattleModel>();
            Games = new ObservableCollection<GameModel>();
            //So stupid
            //Games = StorageHelper.LoadModelsFromJSON<GameModel>(GAMES_FILE_NAME).Result;
            //Doesn't work in vsstudio cause its stupid
            //Battles = StorageHelper.LoadModelsFromJSON<BattleModel>(BATTLES_FILE_NAME).Result;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new Shell();
            Window = m_window;
            Hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window);
            m_window.Activate();
        }

        private Shell m_window;
    }
}
