﻿// GamesPage.xaml.cs
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

using Battle_Assistant.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Views
{
    /// <summary>
    /// The games page
    /// </summary>
    public sealed partial class GamesPage : Page
    {
        public GamesPageViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GamesPage()
        {
            this.InitializeComponent();
            ViewModel = new GamesPageViewModel();
            DataContext = ViewModel; 
        }

        /// <summary>
        /// Add game click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddGame(this.Content.XamlRoot);
        }

        /// <summary>
        /// Delete game click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ViewModel.DeleteGame(int.Parse(btn.Tag.ToString()), this.Content.XamlRoot);
        }
    }
}
