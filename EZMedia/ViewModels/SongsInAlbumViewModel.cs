using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia.ViewModels
{
    public class SongsInAlbumViewModel : INotifyPropertyChanged
    {
        public SongsInAlbumViewModel(AlbumInfo album)
        {
            CurrentAlbumInfo = album;
        }

        private AlbumInfo _currentAlbumInfo;
        public AlbumInfo CurrentAlbumInfo
        {
            get
            {
                return _currentAlbumInfo;
            }
            set
            {
                _currentAlbumInfo = value;
                NotifyPropertyChanged("CurrentAlbumInfo");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
