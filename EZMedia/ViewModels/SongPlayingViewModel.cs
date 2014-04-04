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
using System.Windows.Media.Imaging;
using System.Windows.Input;
using EZMedia.Commands;
using System.Windows.Threading;
using Microsoft.Phone.BackgroundAudio;

namespace EZMedia.ViewModels
{
    public class SongPlayingViewModel : INotifyPropertyChanged
    {

        public SongPlayingViewModel()
        {
            _media_player = new EZMediaPlayer();
            _library = new MediaLibrary();
            _media_player.CurrentMediaChanged += _media_player_CurrentMediaChanged;
            _media_player.MediaStateChanged += _media_player_MediaStateChanged;

            ShuffleSongPictureSource = "/Assets/MusicButtons/ShuffleSongsNotClicked.png";
            RepeatSongPictureSource = "/Assets/MusicButtons/RepeatSongsNotClicked.png";
        }

        private MediaLibrary _library;
        private EZMediaPlayer _media_player;

        private ICommand _playCommand;
        public ICommand PlayCommand 
        {
            get
            {
                return _playCommand;
            }
            private set
            {
                _playCommand = value;
                NotifyPropertyChanged("PlayCommand");
            }
        }
        public ICommand PreviousSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand ShuffleSongsCommand { get; private set; }
        public ICommand RepeatSongsCommand { get; private set; }

        private string _playSongPictureSource;
        public string PlaySongPictureSource
        {
            get
            {
                return _playSongPictureSource;
            }
            set
            {
                _playSongPictureSource = value;
                NotifyPropertyChanged("PlaySongPictureSource");
            }
        }

        private string _shuffleSongPictureSource;
        public string ShuffleSongPictureSource
        {
            get
            {
                return _shuffleSongPictureSource;
            }
            set
            {
                _shuffleSongPictureSource = value;
                NotifyPropertyChanged("ShuffleSongPictureSource");
            }
        }

        private string _repeatSongPictureSource;
        public string RepeatSongPictureSource
        {
            get
            {
                return _repeatSongPictureSource;
            }
            set
            {
                _repeatSongPictureSource = value;
                NotifyPropertyChanged("RepeatSongPictureSource");
            }
        }

        private List<SongInfo> _songs;
        public ReadOnlyCollection<SongInfo> Songs
        {
            get
            {
                return _songs.AsReadOnly();
            }
            private set
            {
                if (value != null)
                {
                    _songs.Clear();
                    _songs.AddRange(value.ToList());
                }
            }
        }
        private SongInfo _currentSongInfo;
        public SongInfo CurrentSongInfo
        {
            get
            {
                return _currentSongInfo;
            }
            private set
            {
                _currentSongInfo = value;
            }
        }

        private BitmapImage _albumArt;
        public BitmapImage AlbumArt
        {
            get
            {
                return _albumArt;
            }
            private set
            {
                _albumArt = value;
                NotifyPropertyChanged("AlbumArt");
            }
        }

        /// <summary>
        /// Method for passing along a song to play and a list of songs to be played after that.
        /// The "song" to play should also be contained in "songsToPlay"
        /// </summary>
        /// <param name="songsToPlay">List of songs to play</param>
        /// <param name="songInfo">The currently selected song to play. This serves as the start point in the collection
        /// "songsToPlay". If it's null, no song will play.</param>
        public void UpdateNowPlayingSong(ReadOnlyCollection<SongInfo> songsToPlay, SongInfo song)
        {
            CurrentSongInfo = song;
            Songs = songsToPlay;
            AlbumArt = findAlbumArt(song);
        }

        private BitmapImage findAlbumArt(SongInfo song)
        {
            BitmapImage image;
            if (song != null)
            {
                Album album = _library.Albums.Where(x => x.Name == song.Album).ToList()[0];
                if (album.HasArt)
                {
                    image = new BitmapImage();
                    image.SetSource(album.GetAlbumArt());
                }
                else
                {
                    image = new BitmapImage(new Uri("/Assets/NoAlbumArt.png", UriKind.Relative));
                }
            }
            else
            {
                image = new BitmapImage(new Uri("/Assets/NoAlbumArt.png", UriKind.Relative));
            }
            return image;
        }

        private void _media_player_MediaStateChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _media_player_CurrentMediaChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
