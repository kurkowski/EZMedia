using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia.ViewModels
{
    public class SongPlayingViewModel : INotifyPropertyChanged
    {
        public SongPlayingViewModel(SongInfo songInfo)
        {
            CurrentSongInfo = songInfo;
        }

        public SongInfo CurrentSongInfo { get; private set; }

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
