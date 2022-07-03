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

using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Battle_Assistant.Helpers
{
    /// <summary>
    /// Settings Helper - Saves and loads settings
    /// </summary>
    public static class SettingsHelper
    {
        private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private const string THEME_SETTING = "theme";

        private const string WIDTH_SETTING = "width";
        private const int MIN_WIDTH = 1000;
        private const string HEIGHT_SETTING = "height";
        private const int MIN_HEIGHT = 550;
        private const string WINDOW_SIZE_SETTING = "windowSize";

        private const string X_POSITION_SETTING = "X";
        private const string Y_POSITION_SETTING = "Y";
        private const string WINDOW_POSITION_SETTING = "position";

        private const string FLASH_TASK_BAR = "flashTaskBar";
        private const string FLASH_AMOUNT = "flashAmount";

        private const string TIPS_EXPLAINED = "tipsExplained";

        /// <summary>
        /// Saves the theme
        /// </summary>
        /// <param name="theme">The theme string</param>
        public static void SaveTheme(string theme)
        {
            localSettings.Values[THEME_SETTING] = theme;
        }

        /// <summary>
        /// Saves the window size
        /// </summary>
        /// <param name="width">The window width</param>
        /// <param name="height">The window height</param>
        public static void SaveWindowSize(int width, int height)
        {
            //Attempt to fix a bug where the window would bug out
            if (width < MIN_WIDTH)
            {
                width = MIN_WIDTH;
            }

            if(height < MIN_HEIGHT)
            {
                height = MIN_HEIGHT;
            }

            ApplicationDataCompositeValue windowSize = new ApplicationDataCompositeValue();
            windowSize[WIDTH_SETTING] = width;
            windowSize[HEIGHT_SETTING] = height;
            localSettings.Values[WINDOW_SIZE_SETTING] = windowSize;
        }

        /// <summary>
        /// Saves the window position
        /// </summary>
        /// <param name="x">The windows x position</param>
        /// <param name="y">The windows y position</param>
        public static void SaveWindowPosition(int x, int y)
        {
            ApplicationDataCompositeValue windowPosition = new ApplicationDataCompositeValue();
            windowPosition[X_POSITION_SETTING] = x;
            windowPosition[Y_POSITION_SETTING] = y;
            localSettings.Values[WINDOW_POSITION_SETTING] = windowPosition;
        }

        /// <summary>
        /// Saves the flash icon in task bar setting
        /// </summary>
        /// <param name="flashIcon">The flash icon in task bar flag</param>
        public static void SaveFlashIcon(bool flashIcon)
        {
            localSettings.Values[FLASH_TASK_BAR] = flashIcon;
        }

        /// <summary>
        /// Saves the flash amount for the flash icon
        /// </summary>
        /// <param name="flashAmount">The amount of times the icon flashes</param>
        public static void SaveFlashAmount(int flashAmount)
        {
            localSettings.Values[FLASH_AMOUNT] = flashAmount;
        }

        /// <summary>
        /// Saves the tips explained flag
        /// </summary>
        /// <param name="tipsExplained"></param>
        public static void SaveTipsExplained(bool tipsExplained)
        {
            localSettings.Values[TIPS_EXPLAINED] = tipsExplained;
        }

        /// <summary>
        /// Loads all the settings, used at application start
        /// </summary>
        public static void LoadSettings()
        {
            //Sets the application theme
            string theme = (string)localSettings.Values[THEME_SETTING];
            if (theme != null)
            {
                (App.MainWindow.Content as Grid).RequestedTheme = EnumHelper.GetEnum<ElementTheme>(theme);
            }

            //Sets the application window size
            ApplicationDataCompositeValue windowSize = (ApplicationDataCompositeValue)localSettings.Values[WINDOW_SIZE_SETTING];
            if (windowSize != null)
            {
                int width = (int)windowSize[WIDTH_SETTING];
                int height = (int)windowSize[HEIGHT_SETTING];
                Debug.WriteLine(width);
                Debug.WriteLine(height);
                if(width > MIN_WIDTH && height > MIN_HEIGHT)
                {
                    App.SetWindowSize(width, height);
                }
            }

            //Sets the application window position
            ApplicationDataCompositeValue windowPosition = (ApplicationDataCompositeValue)localSettings.Values[WINDOW_POSITION_SETTING];
            if (windowPosition != null)
            {
                int x = (int)windowPosition[X_POSITION_SETTING];
                int y = (int)windowPosition[Y_POSITION_SETTING];
                App.SetWindowPosition(x, y);
            }
        }

        /// <summary>
        /// Get the flash icon in the task bar setting
        /// </summary>
        /// <returns>bool of the setting, defaults to true is setting is null</returns>
        public static bool GetFlashIcon()
        {
            bool? flashTaskBar = (bool?)localSettings.Values[FLASH_TASK_BAR];
            if(flashTaskBar != null)
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
            int? flashAmount = (int?)localSettings.Values[FLASH_AMOUNT];
            if(flashAmount != null)
            {
                return (int)flashAmount;
            }
            return 5;
        }

        /// <summary>
        /// Gets the tips explained flag
        /// </summary>
        /// <returns>bool of the tips explained, defaults to false</returns>
        public static bool GetTipsExplained()
        {
            bool? tipsExplained = (bool?)localSettings.Values[TIPS_EXPLAINED];
            if(tipsExplained != null)
            {
                return (bool)tipsExplained;
            }
            return false;
        }
    }
}
