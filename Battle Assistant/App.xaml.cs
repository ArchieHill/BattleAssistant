using Battle_Assistant.Common;
using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using Battle_Assistant.Views;
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
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<BattleModel> Battles { get; set; }

        public static ObservableCollection<GameModel> Games { get; set; }

        public static ObservableCollection<OpponentModel> Opponents { get; set; }

        public static Window MainWindow { get; set; }

        public static IntPtr Hwnd { get; set; }

        private static WindowId windowId;

        private static AppWindow appWindow;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            InitialiseWindow();
        }

        /// <summary>
        /// Loads data and activates the window
        /// </summary>
        private async void InitialiseWindow()
        {
            //Load data, must come before making the main window to ensure the data is loaded onto the default navigated page
            await StorageHelper.LoadAllAsync();

            MainWindow = new NavShell();
            Hwnd = WindowNative.GetWindowHandle(MainWindow);
            windowId = Win32Interop.GetWindowIdFromWindow(Hwnd);
            appWindow = AppWindow.GetFromWindowId(windowId);

            //Load settings
            SettingsHelper.LoadSettings();

            //We set this after the load settings so we don't waste time writing the sizes to the settings that we just loaded
            appWindow.Changed += AppWindow_Changed;

            //Checks if battles have changed whilst the application has been closed
            UpdateAllBattles();

            MainWindow.Activate();
        }

        /// <summary>
        /// Resizes the application window
        /// </summary>
        /// <param name="width">The new width of the application</param>
        /// <param name="height">The new height of the application</param>
        public static void SetWindowSize(int width, int height)
        {
            var size = new SizeInt32();
            size.Width = width;
            size.Height = height;
            appWindow.Resize(size);    
        }

        /// <summary>
        /// Repositions the application window
        /// </summary>
        /// <param name="x">The x position on the screen</param>
        /// <param name="y">The y position on the screen</param>
        public static void SetWindowPosition(int x, int y)
        {
            var position = new PointInt32();
            position.X = x;
            position.Y = y;
            appWindow.Move(position);
        }

        /// <summary>
        /// App Window property change event
        /// </summary>
        /// <param name="sender">The app window</param>
        /// <param name="args">The event args</param>
        private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
        {
            if (args.DidSizeChange)
            {
                var size = sender.Size;
                SettingsHelper.SaveWindowSize(size.Width, size.Height);
            }

            if(args.DidPositionChange)
            {
                var position = sender.Position;
                SettingsHelper.SaveWindowPosition(position.X, position.Y);
            }
        }

        /// <summary>
        /// Checks each battle to see if there is a new file to move for the battle
        /// </summary>
        private void UpdateAllBattles()
        {
            foreach (BattleModel battle in Battles)
            {
                if (battle.Status == Status.WAITING)
                {
                    //This constructs the path of the file with the next file number, then checks if its exists
                    string nextBattleFilePath = FileHelper.ConstructBattleFilePath(battle.Opponent.SharedDir, battle.Name, battle.CurrentFileNum + 1);
                    if (File.Exists(nextBattleFilePath))
                    {
                        battle.BattleFile = nextBattleFilePath;
                        FileHelper.CopyToIncomingEmail(battle);
                    }
                }
                else if (battle.Status == Status.YOUR_TURN)
                {
                    //This constructs the path of the file with the next file number, then checks if its exists
                    string nextBattleFilePath = FileHelper.ConstructBattleFilePath(battle.Game.OutgoingEmailFolder, battle.Name, battle.CurrentFileNum + 1);
                    if (File.Exists(nextBattleFilePath))
                    {
                        battle.BattleFile = nextBattleFilePath;
                        FileHelper.CopyToSharedDrive(battle);
                    }
                }

            }
        }
    }
}
