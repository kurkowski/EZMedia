using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EZMedia.ViewModels;

namespace EZMedia.Views
{
    public partial class SelectPlaylists : PhoneApplicationPage
    {
        IApplicationBar PlaylistPivotDeleteAppBar;

        public SelectPlaylists()
        {
            InitializeComponent();
            DataContext = App.ViewModel.PlaylistsVM;
            makeDeleteAppBar();
            ApplicationBar = PlaylistPivotDeleteAppBar;
        }

        private void makeDeleteAppBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            PlaylistPivotDeleteAppBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
            appBarButton.Text = "delete";
            appBarButton.Click += appBarButton_Click;
            PlaylistPivotDeleteAppBar.Buttons.Add(appBarButton);
        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.PlaylistsVM.RemovePlaylists();
            NavigationService.GoBack();
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            ItemWithCheckBox item = sp.DataContext as ItemWithCheckBox;
            if (item.IsChecked)
            {
                item.IsChecked = false;
            }
            else
            {
                item.IsChecked = true;
            }
        }
    }
}