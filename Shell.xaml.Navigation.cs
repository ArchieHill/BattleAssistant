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
    public partial class Shell : INavigation
    {
        private void NavView_Loaded(object sender, RoutedEventArgs args)
        {
            SetCurrentNavigationViewItem(GetNavigationViewItems(typeof(BattlesPage)).First());
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem, args.IsSettingsSelected);   
        }

        public NavigationViewItem GetCurrentNavigationViewItem()
        {
            return NavView.SelectedItem as NavigationViewItem;
        }

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

        public List<NavigationViewItem> GetNavigationViewItems(Type type, string title)
        {
            return GetNavigationViewItems(type).Where(ni => ni.Content.ToString() == title).ToList();
        }

        public List<NavigationViewItem> GetNavigationViewItems(Type type)
        {
            return GetNavigationViewItems().Where(i => i.Tag.ToString() == type.FullName).ToList();
        }

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
