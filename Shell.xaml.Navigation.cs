using Battle_Assistant.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant
{
    /// <summary>
    /// The navigation part of the shell
    /// </summary>
    public partial class Shell : INavigation
    {
        /// <summary>
        /// Sets the first page selected to be the battle page when the nav is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavView_Loaded(object sender, RoutedEventArgs args)
        {
            SetCurrentNavigationViewItem(GetNavigationViewItems(typeof(BattlesPage)).First());
        }

        /// <summary>
        /// Sets the page to the navigation that was selected
        /// </summary>
        /// <param name="sender">The navigation view</param>
        /// <param name="args">The selection change data</param>
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem, args.IsSettingsSelected);   
        }

        /// <summary>
        /// Gets the current selected nav view item
        /// </summary>
        /// <returns>The current selected navigation view item</returns>
        public NavigationViewItem GetCurrentNavigationViewItem()
        {
            return NavView.SelectedItem as NavigationViewItem;
        }

        /// <summary>
        /// Gets a list of all the navigation view items
        /// </summary>
        /// <returns>A list of navigation view items</returns>
        public List<NavigationViewItem> GetNavigationViewItems()
        {
            List<NavigationViewItem> result = new();
            var items = NavView.MenuItems.Select(i => (NavigationViewItem)i).ToList();
            items.AddRange(NavView.FooterMenuItems.Select(i => (NavigationViewItem)i));
            result.AddRange(items);

            foreach (NavigationViewItem mainItem in items)
            {
                result.AddRange(mainItem.MenuItems.Select(i => (NavigationViewItem)i));
            }

            return result;
        }

        /// <summary>
        /// Gets a list of navigation view items based on the page type
        /// </summary>
        /// <param name="type">The page type</param>
        /// <returns>A list of navigation view items that are the page type</returns>
        public List<NavigationViewItem> GetNavigationViewItems(Type type)
        {
            return GetNavigationViewItems().Where(i => i.Tag.ToString() == type.FullName).ToList();
        }

        /// <summary>
        /// Gets a list of the navigation view items based on the type and title of the page
        /// </summary>
        /// <param name="type">The type of page</param>
        /// <param name="title">The title of the page</param>
        /// <returns>A list of navigation view items that are the page type and title</returns>
        public List<NavigationViewItem> GetNavigationViewItems(Type type, string title)
        {
            return GetNavigationViewItems(type).Where(ni => ni.Content.ToString() == title).ToList();
        }

        /// <summary>
        /// Navigates to the page based on the inputted navigation view item
        /// </summary>
        /// <param name="item">The navigation view item</param>
        public void SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.Tag == null)
            {
                return;
            }

            ContentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Content);
            NavView.Header = item.Content;
            NavView.SelectedItem = item;
        }

        /// <summary>
        /// Navigates to the page and and checks if the settings page has been selected
        /// </summary>
        /// <param name="item">The navigation view item</param>
        /// <param name="SettingsSelected">Boolean if the setting page is selected</param>
        public void SetCurrentNavigationViewItem(NavigationViewItem item, bool SettingsSelected)
        {
            string tag;

            if (item == null)
            {
                return;
            }

            if (SettingsSelected)
            {
                tag = "Battle_Assistant.Views.SettingsPage";
            }
            else if(item.Tag == null)
            {
                return;
            }
            else
            {
                tag = item.Tag.ToString();
            }

            ContentFrame.Navigate(Type.GetType(tag), item.Content);
            NavView.Header = item.Content;
            NavView.SelectedItem = item;
        }
    }
}
