using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EZMedia.Views
{
    public partial class SongsForArtist : PhoneApplicationPage
    {
        public SongsForArtist()
        {
            InitializeComponent();
            DataContext = App.ViewModel.SongsForArtistVM;
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            SongInfo song = sp.DataContext as SongInfo;
            ArtistInfo artist = App.ViewModel.SongsForArtistVM.CurrentArtistInfo;
            App.ViewModel.SongPlayingVM.UpdateNowPlayingSong(artist, song);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void StackPanel_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            AlbumInfo album = (AlbumInfo)sp.Tag;
            App.ViewModel.SongsForArtistVM.SongInAlbumVM = new ViewModels.SongsInAlbumViewModel(album);
            App.AlbumViewModel = App.ViewModel.SongsForArtistVM.SongInAlbumVM;
            NavigationService.Navigate(new Uri("/Views/SongsInAlbum.xaml", UriKind.Relative));
        }
    }
}