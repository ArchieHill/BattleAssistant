// DeleteConfirmationDialog.xaml.cs
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

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BattleAssistant.Dialogs
{
    /// <summary>
    /// A delete confirmation dialog
    /// </summary>
    public sealed partial class DeleteConfirmationDialog : ContentDialog
    {
        private bool deleteAllowed;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteConfirmationDialog(bool deleteAllowed)
        {
            this.InitializeComponent();
            this.deleteAllowed = deleteAllowed;
            Loaded += Dialog_Loaded;
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs args)
        {
            if (!deleteAllowed)
            {
                DialogInfoBar.Severity = InfoBarSeverity.Error;
                DialogInfoBar.Title = "Cannot Delete";
                DialogInfoBar.Message = "Cannot be deleted as a battle has this, end the battles to allow this to be deleted";
                DialogInfoBar.IsOpen = true;
                IsPrimaryButtonEnabled = false;
            }
        }
    }
}
