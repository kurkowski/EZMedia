using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia.ViewModels
{
    public class SongsForArtistViewModel : INotifyPropertyChanged
    {
        public SongsForArtistViewModel(ArtistInfo artistInfo)
        {
            CurrentArtistInfo = artistInfo;
            _GroupedSongList = StringKeyGroup<IMediaInfo>.CreateGroups(artistInfo.Songs);
        }

        public SongsInAlbumViewModel SongInAlbumVM { get; set; }

        public ArtistInfo CurrentArtistInfo { get; set; }

        private List<StringKeyGroup<IMediaInfo>> _GroupedSongList;
        public List<StringKeyGroup<IMediaInfo>> GroupedSongList
        {
            get { return _GroupedSongList; }
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
