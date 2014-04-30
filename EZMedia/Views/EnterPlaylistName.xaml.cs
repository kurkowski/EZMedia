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
    public partial class EnterPlaylistName : PhoneApplicationPage
    {
        public EnterPlaylistName()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PlaylistsVM.AddNewPlaylist(NameInput.Text);
            NavigationService.GoBack();
        }
    }
}