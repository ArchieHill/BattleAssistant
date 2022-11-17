// SettingsPageViewModel.cs
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

using BattleAssistant.Common;
using BattleAssistant.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace BattleAssistant.ViewModels
{
    public class SettingsPageViewModel : ObservableObject
    {
        private NumberBox flashAmountBox;

        private ToggleSwitch autoCreateOpponentSwitch;

        private ToggleSwitch autoCreateGameSwitch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="autoCreateOpponentSwitch"></param>
        /// <param name="autoCreateGameSwitch"></param>
        public SettingsPageViewModel(NumberBox flashAmountBox, ToggleSwitch autoCreateOpponentSwitch, ToggleSwitch autoCreateGameSwitch)
        {
            this.flashAmountBox = flashAmountBox;
            this.autoCreateOpponentSwitch = autoCreateOpponentSwitch;
            this.autoCreateGameSwitch = autoCreateGameSwitch;
        }

        private bool flashIcon;
        public bool FlashIcon
        {
            get { return flashIcon; }
            set
            {
                if (flashIcon != value)
                {
                    flashIcon = value;
                    SettingsHelper.SaveFlashIcon(flashIcon);
                    NotifyPropertyChanged();

                    if (flashIcon)
                    {
                        flashAmountBox.IsEnabled = true;
                    }
                    else
                    {
                        flashAmountBox.IsEnabled = false;
                    }
                }
            }
        }

        private int flashAmount;
        public int FlashAmount
        {
            get
            {
                return flashAmount;
            }
            set
            {
                if (flashAmount != value)
                {
                    flashAmount = value;
                    SettingsHelper.SaveFlashAmount(flashAmount);
                    NotifyPropertyChanged();
                }
            }
        }

        private bool autoSelectOpponent;
        public bool AutoSelectOpponent
        {
            get { return autoSelectOpponent; }
            set
            {
                if (autoSelectOpponent != value)
                {
                    autoSelectOpponent = value;
                    SettingsHelper.SaveAutoSelectOpponent(autoSelectOpponent);
                    NotifyPropertyChanged();

                    if (autoSelectOpponent)
                    {
                        autoCreateOpponentSwitch.IsEnabled = true;
                    }
                    else
                    {
                        AutoCreateOpponent = false;
                        autoCreateGameSwitch.IsOn = false;
                        autoCreateOpponentSwitch.IsEnabled = false;
                    }
                }
            }
        }

        private bool autoCreateOpponent;
        public bool AutoCreateOpponent
        {
            get { return autoCreateOpponent; }
            set
            {
                if (autoCreateOpponent != value)
                {
                    autoCreateOpponent = value;
                    SettingsHelper.SaveAutoCreateOpponent(autoCreateOpponent);
                    NotifyPropertyChanged();
                }
            }
        }

        private bool autoSelectGame;
        public bool AutoSelectGame
        {
            get { return autoSelectGame; }
            set
            {
                if (autoSelectGame != value)
                {
                    autoSelectGame = value;
                    SettingsHelper.SaveAutoSelectGame(autoSelectGame);
                    NotifyPropertyChanged();

                    if (autoSelectGame)
                    {
                        autoCreateGameSwitch.IsEnabled = true;
                    }
                    else
                    {
                        AutoCreateGame = false;
                        autoCreateGameSwitch.IsOn = false;
                        autoCreateGameSwitch.IsEnabled = false;
                    }
                }
            }
        }

        private bool autoCreateGame;
        public bool AutoCreateGame
        {
            get { return autoCreateGame; }
            set
            {
                if (autoCreateGame != value && AutoSelectGame)
                {
                    autoCreateGame = value;
                    SettingsHelper.SaveAutoCreateGame(autoCreateGame);
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
