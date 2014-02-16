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
        }

        public ArtistInfo CurrentArtistInfo { get; set; }

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
