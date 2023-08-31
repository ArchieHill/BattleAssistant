// App.xaml.cs
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

using System;
using System.Collections.ObjectModel;
using System.IO;
using BattleAssistant.Common;
using BattleAssistant.DialogModels;
using BattleAssistant.Helpers;
using BattleAssistant.Interfaces;
using BattleAssistant.Models;
using BattleAssistant.Services;
using BattleAssistant.ViewModels;
using BattleAssistant.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Serilog;
using Serilog.Events;
using Windows.Graphics;
using Windows.Media.AppBroadcasting;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BattleAssistant
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private const int MinHeight = 440;
        private const int MinWidth = 650;

        public static IHost Application;

        public static NavShell MainWindow;

        public static XamlRoot Root;

        public static IntPtr Hwnd { get; set; }

        public static AppWindow AppWindow { get; set; }

        private static WindowId windowId;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            string logsDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BattleAssistant", "Logs");           
            Directory.CreateDirectory(logsDirectoryPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.File(Path.Combine(logsDirectoryPath, "log.txt"), LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Application = SetupApplication();
            Application.Start();
        }

        private IHost SetupApplication()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            //Services
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<IStorageService, StorageService>();

            //ViewModels
            builder.Services.AddSingleton<BattlesPageViewModel>();
            builder.Services.AddSingleton<GamesPageViewModel>();
            builder.Services.AddSingleton<OpponentsPageViewModel>();
            builder.Services.AddSingleton<SettingsPageViewModel>();
            builder.Services.AddSingleton<AddGameDialogModel>();
            builder.Services.AddSingleton<AddOpponentDialogModel>();
            builder.Services.AddSingleton<EndBattleConfirmationDialogModel>();
            builder.Services.AddSingleton<StartBattleDialogModel>();

            //Views
            builder.Services.AddSingleton<BattlesPage>();
            builder.Services.AddSingleton<GamesPage>();
            builder.Services.AddSingleton<OpponentsPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddSingleton<GettingStartedPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<NavShell>();

            return builder.Build();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = Application.Services.GetService<NavShell>();
            Root = MainWindow.Content.XamlRoot;
            Hwnd = WindowNative.GetWindowHandle(MainWindow);
            windowId = Win32Interop.GetWindowIdFromWindow(Hwnd);
            AppWindow = AppWindow.GetFromWindowId(windowId);

            //We set this after the load settings so we don't waste time writing the sizes to the settings that we just loaded
            AppWindow.Changed += AppWindow_Changed;

            //Checks if battles have changed whilst the application has been closed
            UpdateAllBattles();

            Log.Information("Application ready, activating main window");
            MainWindow.Activate();
        }

        /// <summary>
        /// Adds an opponent to the application
        /// </summary>
        /// <param name="opponent">The opponent model to add</param>
        public static void AddOpponent(OpponentModel opponent)
        {
            IStorageService storageService = Application.Services.GetService<IStorageService>();

            storageService.Opponents.Add(opponent);
            //Assign its index so we know where to look to delete it
            opponent.Index = storageService.Opponents.IndexOf(opponent);
            storageService.UpdateOpponentFile();
        }

        /// <summary>
        /// Adds a game to the application
        /// </summary>
        /// <param name="game">The game model to add</param>
        public static void AddGame(GameModel game)
        {
            IStorageService storageService = Application.Services.GetService<IStorageService>();

            storageService.Games.Add(game);
            //Assign its index so we know where to look to delete it
            game.Index = storageService.Games.IndexOf(game);
            storageService.UpdateGameFile();
        }

        /// <summary>
        /// Resizes the application window
        /// </summary>
        /// <param name="width">The new width of the application</param>
        /// <param name="height">The new height of the application</param>
        public static void SetWindowSize(int width, int height)
        {
            var size = new SizeInt32
            {
                Width = width,
                Height = height
            };
            AppWindow.Resize(size);
        }

        /// <summary>
        /// App Window property change event
        /// </summary>
        /// <param name="sender">The app window</param>
        /// <param name="args">The event args</param>
        private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
        {
            ISettingsService settingsService = Application.Services.GetService<ISettingsService>();

            if (args.DidSizeChange)
            {
                var size = sender.Size;

                if(size.Height < MinHeight)
                {
                    size.Height = MinHeight;
                }

                if (size.Width < MinWidth)
                {
                    size.Width = MinWidth;
                }

                settingsService.SaveWindowSize(size.Width, size.Height);
            }
        }

        /// <summary>
        /// Checks each battle to see if there is a new file to move for the battle
        /// </summary>
        private static async void UpdateAllBattles()
        {
            IFileService fileService = Application.Services.GetRequiredService<IFileService>();
            IStorageService storageService = Application.Services.GetService<IStorageService>();

            foreach (BattleModel battle in storageService.Battles)
            {
                if (battle.Status == Status.Waiting)
                {
                    //This constructs the path of the file with the next file number, then checks if its exists
                    string nextBattleFilePath = BattleFileHelper.ConstructFilePath(battle.Opponent.SharedDir, battle.Name, battle.CurrentFileNum + 1);
                    if (File.Exists(nextBattleFilePath))
                    {
                        battle.BattleFile = nextBattleFilePath;
                        await fileService.CopyToIncomingEmailAsync(battle);
                    }
                }
                else if (battle.Status == Status.YourTurn)
                {
                    //This constructs the path of the file with the next file number, then checks if its exists
                    string nextBattleFilePath = BattleFileHelper.ConstructFilePath(battle.Game.OutgoingEmailFolder, battle.Name, battle.CurrentFileNum + 1);
                    if (File.Exists(nextBattleFilePath))
                    {
                        battle.BattleFile = nextBattleFilePath;
                        await fileService.CopyToSharedDriveAsync(battle);
                    }
                }
            }
            Log.Information("All battles updated");
        }
    }
}
