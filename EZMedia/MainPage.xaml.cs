using System;
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
        IApplicationBar SongPivotAppBar;
        IApplicationBar PlaylistPivotSelectAppBar;


        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            NowPlayingPivot.DataContext = App.ViewModel.SongPlayingVM;
            PlaylistsPivot.DataContext = App.ViewModel.PlaylistsVM;
            // Sample code to localize the ApplicationBar
            BuildSongPivotAppBar();
            BuildAppBarForPlaylistsSelect();
            ApplicationBar = SongPivotAppBar;
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
            ReadOnlyCollection<SongInfo> songs = (ReadOnlyCollection<SongInfo>)SongLLS.Tag;

            StackPanel sp = (StackPanel)sender;
            TextBlock tb = (TextBlock)e.OriginalSource;
            SongInfo songToPlay = (SongInfo)tb.DataContext;
            EZMediaPivot.SelectedIndex = 0;
            App.ViewModel.SongPlayingVM.UpdateNowPlayingSong(songs, songToPlay);
            NowPlayingPivot.DataContext = App.ViewModel.SongPlayingVM;
        }

        private void highlightCurrentSong()
        {
            if (App.ViewModel.SongPlayingVM.Songs.Count > 0)
            {
                int index = App.ViewModel.SongPlayingVM.CurrentSongIndex;
                NowPlayingList.UpdateLayout();
                NowPlayingList.ScrollIntoView(NowPlayingList.Items[index]);
                NowPlayingList.SelectedIndex = index;
            }
        }

        private void NowPlayingList_LayoutUpdated(object sender, EventArgs e)
        {
            highlightCurrentSong();
        }

        private void LongListSelectorArtists_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel lls = (StackPanel)sender;
            ArtistInfo artInfo = (ArtistInfo)lls.Tag;
            App.ViewModel.SongsForArtistVM = new SongsForArtistViewModel(artInfo);
            NavigationService.Navigate(new Uri("/Views/SongsForArtist.xaml", UriKind.Relative));
        }

        private void LongListSelectorAlbums_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel lls = (StackPanel)sender;
            AlbumInfo ai = (AlbumInfo)lls.Tag;
            App.ViewModel.SongsInAlbumVM = new SongsInAlbumViewModel(ai);
            App.AlbumViewModel = App.ViewModel.SongsInAlbumVM;
            NavigationService.Navigate(new Uri("/Views/SongsInAlbum.xaml", UriKind.Relative));
        }

        private void EZMediaPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = false;
                    break;
                case 1: //SongPivot
                    ApplicationBar = SongPivotAppBar;
                    ApplicationBar.IsVisible = true;
                    break;
                case 4: //PlaylistPivot
                    ApplicationBar = PlaylistPivotSelectAppBar;
                    ApplicationBar.IsVisible = true;
                    break;
                default:
                    ApplicationBar.IsVisible = false;
                    break;
            }
        }

        private void QueueStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        //Sample code for building a localized ApplicationBar
        private void BuildSongPivotAppBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            SongPivotAppBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarButtonText;
            appBarButton.Click += appBarSelectSongButton_Click;
            SongPivotAppBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            SongPivotAppBar.MenuItems.Add(appBarMenuItem);
        }

        void appBarSelectSongButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BuildAppBarForPlaylistsSelect()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            PlaylistPivotSelectAppBar = new ApplicationBar();

            ApplicationBarIconButton createButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            createButton.Text = "create";
            createButton.Click += NewPlaylistButton_Tap;
            PlaylistPivotSelectAppBar.Buttons.Add(createButton);

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/check.png", UriKind.Relative));
            appBarButton.Text = "select";
            appBarButton.Click += appBarSelectButton_Click;
            PlaylistPivotSelectAppBar.Buttons.Add(appBarButton);
        }

        void appBarSelectButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SelectPlaylists.xaml", UriKind.Relative));
        }

        private void NewPlaylistButton_Tap(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/EnterPlaylistName.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Play selected playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaylistStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        
    }
}