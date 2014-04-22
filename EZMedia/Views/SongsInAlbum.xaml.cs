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
    public partial class SongsInAlbum : PhoneApplicationPage
    {
        public SongsInAlbum()
        {
            InitializeComponent();
            DataContext = App.AlbumViewModel;
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            SongInfo song = sp.DataContext as SongInfo;
            AlbumInfo album = App.AlbumViewModel.CurrentAlbumInfo;
            App.ViewModel.SongPlayingVM.UpdateNowPlayingSong(album, song);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


    }
}