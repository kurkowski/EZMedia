﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EZMedia.Resources;
using EZMedia.Views;
using EZMedia.ViewModels;
using System.Collections.ObjectModel;

namespace EZMedia
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LongListSelectorSongs_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LongListSelector lls = (LongListSelector)sender;
            ReadOnlyCollection<SongInfo> songs = (ReadOnlyCollection<SongInfo>)lls.Tag;
            TextBlock tb = (TextBlock)e.OriginalSource;
            SongInfo songToPlay = (SongInfo)tb.DataContext;
            App.ViewModel.SongPlayingVM = new SongPlayingViewModel(songs, songToPlay);
            NavigationService.Navigate(new Uri("/Views/SongPlayingPage.xaml", UriKind.Relative));
        }

        private void LongListSelectorArtists_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LongListSelector lls = (LongListSelector)sender;
            ArtistInfo artInfo = (ArtistInfo)lls.Tag;

            //TODO: MAKE ARTIST PAGE THAT DISPLAYS ALL ALBUMS

            //App.ViewModel.SongPlayingVM = new SongPlayingViewModel(songs, songToPlay);
            //NavigationService.Navigate(new Uri("/Views/SongPlayingPage.xaml", UriKind.Relative));
        }

        private void LongListSelectorAlbums_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LongListSelector lls = (LongListSelector)sender;
            AlbumInfo ai = (AlbumInfo)lls.Tag;

            //TODO: MAKE ALBUM PAGE THAT DISPLAYS ALBUM ART AND ALL SONGS ON ALBUM

            //App.ViewModel.SongPlayingVM = new SongPlayingViewModel(ai.Songs, ai);
            //NavigationService.Navigate(new Uri("/Views/SongPlayingPage.xaml", UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}