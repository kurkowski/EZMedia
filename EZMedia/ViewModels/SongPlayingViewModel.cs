using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.ObjectModel;

namespace EZMedia.ViewModels
{
    public class SongPlayingViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for passing along a song to play and a list of songs to be played after that.
        /// The "song" to play should also be contained in "songsToPlay"
        /// </summary>
        /// <param name="songsToPlay">List of songs to play</param>
        /// <param name="songInfo">The currently selected song to play. This serves as the start point in the collection "songsToPlay".</param>
        public SongPlayingViewModel(ReadOnlyCollection<SongInfo> songsToPlay, SongInfo song)
        {
            CurrentSongInfo = song;
        }

        /// <summary>
        /// A specific album to play. The album is provided 
        /// </summary>
        /// <param name="songsToPlay"></param>
        /// <param name="album"></param>
        public SongPlayingViewModel(ReadOnlyCollection<SongInfo> songsToPlay, AlbumInfo album)
        {

        }

        /// <summary>
        /// Plays all songs/albums from a specific artist
        /// </summary>
        /// <param name="songsToPlay"></param>
        /// <param name="album"></param>
        public SongPlayingViewModel(ReadOnlyCollection<SongInfo> songsToPlay, ArtistInfo artist)
        {

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
