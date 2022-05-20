﻿using Battle_Assistant.DialogModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Battle_Assistant.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddGameDialog : ContentDialog
    {
        AddGameDialogModel DialogModel { get; set; } = new AddGameDialogModel();
        public AddGameDialog()
        {
            this.InitializeComponent();
            DataContext = DialogModel;
        }

        private void AddGame_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DialogModel.AddGame();
        }

        public void SelectGameDir_Click(object sender, RoutedEventArgs e)
        {
            DialogModel.SelectGameDir();
        }

        public void SelectIconFile_Click(object sender, RoutedEventArgs e)
        {
            DialogModel.SelectIconFile();
        }
    }
}
