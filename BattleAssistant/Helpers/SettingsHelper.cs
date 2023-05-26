// SettingsHelper.cs
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
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace BattleAssistant.Helpers
{
    /// <summary>
    /// Settings Helper - Saves and loads settings
    /// </summary>
    public static class SettingsHelper
    {
        private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private const string Theme = "theme";

        private const string Width = "width";
        private const int MinWidth = 1000;
        private const string Height = "height";
        private const int MinHeight = 550;
        private const string WindowSize = "windowSize";

        private const string AutoSelectOpponent = "autoSelectOpponent";
        private const string AutoCreateOpponent = "autoCreateOpponent";
        private const string BackupFolderPath = "backupFolderPath";
        private const string AutoSelectGame = "autoSelectGame";
        private const string AutoCreateGame = "autoCreateGame";

        private const string FlashTaskBar = "flashTaskBar";
        private const string FlashAmount = "flashAmount";

        /// <summary>
        /// Saves the theme
        /// </summary>
        /// <param name="theme">The theme string</param>
        public static void SaveTheme(string theme)
        {
            localSettings.Values[Theme] = theme;
        }

        /// <summary>
        /// Saves the window size
        /// </summary>
        /// <param name="width">The window width</param>
        /// <param name="height">The window height</param>
        public static void SaveWindowSize(int width, int height)
        {
            ApplicationDataCompositeValue windowSize = new()
            {
                [Width] = width,
                [Height] = height
            };
            localSettings.Values[WindowSize] = windowSize;
        }

        /// <summary>
        /// Saves the backup folder path
        /// </summary>
        /// <param name="path">The path</param>
        public static void SaveBackupFolderPath(string path)
        {
            localSettings.Values[BackupFolderPath] = path;
        }

        /// <summary>
        /// Saves the auto select opponent
        /// </summary>
        /// <param name="autoSelect">The auto select value</param>
        public static void SaveAutoSelectOpponent(bool autoSelect)
        {
            localSettings.Values[AutoSelectOpponent] = autoSelect;
        }

        /// <summary>
        /// Saves the auto create opponent
        /// </summary>
        /// <param name="autoCreate">The auto create value</param>
        public static void SaveAutoCreateOpponent(bool autoCreate)
        {
            localSettings.Values[AutoCreateOpponent] = autoCreate;
        }

        /// <summary>
        /// Saves the auto select game
        /// </summary>
        /// <param name="autoSelect">The auto select value</param>
        public static void SaveAutoSelectGame(bool autoSelect)
        {
            localSettings.Values[AutoSelectGame] = autoSelect;
        }

        /// <summary>
        /// Saves the auto create game
        /// </summary>
        /// <param name="autoCreate">The auto create value</param>
        public static void SaveAutoCreateGame(bool autoCreate)
        {
            localSettings.Values[AutoCreateGame] = autoCreate;
        }

        /// <summary>
        /// Saves the flash icon in task bar setting
        /// </summary>
        /// <param name="flashIcon">The flash icon in task bar flag</param>
        public static void SaveFlashIcon(bool flashIcon)
        {
            localSettings.Values[FlashTaskBar] = flashIcon;
        }

        /// <summary>
        /// Saves the flash amount for the flash icon
        /// </summary>
        /// <param name="flashAmount">The amount of times the icon flashes</param>
        public static void SaveFlashAmount(int flashAmount)
        {
            localSettings.Values[FlashAmount] = flashAmount;
        }

        /// <summary>
        /// Loads all the settings, used at application start
        /// </summary>
        public static void LoadSettings()
        {
            //Sets the application theme
            string theme = (string)localSettings.Values[Theme];
            if (theme != null)
            {
                (App.MainWindow.Content as Grid).RequestedTheme = EnumHelper.GetEnum<ElementTheme>(theme);
            }

            //Sets the application window size
            ApplicationDataCompositeValue windowSize = (ApplicationDataCompositeValue)localSettings.Values[WindowSize];
            if (windowSize != null)
            {
                int width = (int)windowSize[Width];
                int height = (int)windowSize[Height];
                if (width > MinWidth && height > MinHeight)
                {
                    App.SetWindowSize(width, height);
                }
            }

            
        }

        /// <summary>
        /// Get the backup folder path
        /// </summary>
        /// <returns>The folder path</returns>
        public static string GetBackupFolderPath()
        {
            string backupFolderPath = (string)localSettings.Values[BackupFolderPath];
            if (backupFolderPath != null | backupFolderPath == "")
            {
                return backupFolderPath;
            }
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return $@"{docPath}\Battlefront\Combat Mission\Battle Backups";
        }

        /// <summary>
        /// Get the auto select opponent setting
        /// </summary>
        /// <returns>bool of the setting, defaults to true is setting is null</returns>
        public static bool GetAutoSelectOpponent()
        {
            bool? autoSelectOpponent = (bool?)localSettings.Values[AutoSelectOpponent];
            if (autoSelectOpponent != null)
            {
                return (bool)autoSelectOpponent;
            }
            return true;
        }


        /// <summary>
        /// Get the auto create opponent setting
        /// </summary>
        /// <returns>bool of the setting, defaults to false is setting is null</returns>
        public static bool GetAutoCreateOpponent()
        {
            bool? autoCreateOpponent = (bool?)localSettings.Values[AutoCreateOpponent];
            if (autoCreateOpponent != null)
            {
                return (bool)autoCreateOpponent;
            }
            return false;
        }

        /// <summary>
        /// Get the auto select game setting
        /// </summary>
        /// <returns>bool of the setting, defaults to true is setting is null</returns>
        public static bool GetAutoSelectGame()
        {
            bool? autoSelectGame = (bool?)localSettings.Values[AutoSelectGame];
            if (autoSelectGame != null)
            {
                return (bool)autoSelectGame;
            }
            return true;
        }

        /// <summary>
        /// Get the auto create game setting
        /// </summary>
        /// <returns>bool of the setting, defaults to false is setting is null</returns>
        public static bool GetAutoCreateGame()
        {
            bool? autoCreateGame = (bool?)localSettings.Values[AutoCreateGame];
            if (autoCreateGame != null)
            {
                return (bool)autoCreateGame;
            }
            return false;
        }

        /// <summary>
        /// Get the flash icon in the task bar setting
        /// </summary>
        /// <returns>bool of the setting, defaults to true is setting is null</returns>
        public static bool GetFlashIcon()
        {
            bool? flashTaskBar = (bool?)localSettings.Values[FlashTaskBar];
            if (flashTaskBar != null)
            {
                return (bool)flashTaskBar;
            }
            return true;
        }

        /// <summary>
        /// Gets the flash amount for the flash icon
        /// </summary>
        /// <returns>int of the setting, defaults to 5 is setting is null</returns>
        public static int GetFlashAmount()
        {
            int? flashAmount = (int?)localSettings.Values[FlashAmount];
            if (flashAmount != null)
            {
                return (int)flashAmount;
            }
            return 5;
        }
    }
}
