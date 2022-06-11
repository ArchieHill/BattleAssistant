using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const string HEIGHT_SETTING = "height";
        private const string WINDOW_SIZE_SETTING = "windowSize";

        /// <summary>
        /// Saves the theme
        /// </summary>
        /// <param name="theme">The theme string</param>
        public static void SaveTheme(string theme)
        {
            localSettings.Values[THEME_SETTING] = theme;
        }

        public static void SaveWindowSize(int width, int height)
        {
            ApplicationDataCompositeValue windowSize = new ApplicationDataCompositeValue();
            windowSize[WIDTH_SETTING] = width;
            windowSize[HEIGHT_SETTING] = height;
            localSettings.Values[WINDOW_SIZE_SETTING] = windowSize;
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
            if(windowSize != null)
            {
                int width = (int)windowSize[WIDTH_SETTING];
                int height = (int)windowSize[HEIGHT_SETTING];
                App.SetWindowSize(width, height);
            }
        }


    }
}
