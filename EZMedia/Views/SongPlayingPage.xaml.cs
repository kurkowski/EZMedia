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
    public partial class SongPlayingPage : PhoneApplicationPage
    {
        public SongPlayingPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.SongPlayingVM;
        }
    }
}