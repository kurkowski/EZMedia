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
            //initialize fields
            _media_player = new EZMediaPlayer();
            _library = new MediaLibrary();
            _songs = new List<SongInfo>();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);

            //subscribe to events
            _timer.Tick += _timer_Tick;
            _media_player.CurrentMediaChanged += _media_player_CurrentMediaChanged;
            _media_player.MediaStateChanged += _media_player_MediaStateChanged;

            //initialize properties
            PlayCommand = new PlayPauseCommand(_media_player);

            ShuffleSongPictureSource = "/Assets/MusicButtons/ShuffleSongsNotClicked.png";
            RepeatSongPictureSource = "/Assets/MusicButtons/RepeatSongsNotClicked.png";
            PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
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
                NotifyPropertyChanged("CurrentSongInfo");
            }
        }
        private DispatcherTimer _timer;
        private TimeSpan _currentTimeOfSong;

        private string _songTime;
        public string SongTime
        {
            get
            {
                return _songTime;
            }
            private set
            {
                _songTime = value + " / " + CurrentSongInfo.Duration.ToString(@"hh\:mm\:ss");
                NotifyPropertyChanged("SongTime");
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
            Songs = songsToPlay;
            updateSongInfo(song);
            _media_player.Play(songsToPlay, song);
        }

        public void UpdateNowPlayingSong(AlbumInfo album, SongInfo song)
        {
            Songs = album.Songs;
            updateSongInfo(song);
            _media_player.Play(album, song);
        }

        public void UpdateNowPlayingSong(ArtistInfo artist, SongInfo song)
        {
            Songs = artist.Songs;
            updateSongInfo(song);
            _media_player.Play(artist, song);
        }

        public void UpdateNowPlayingSong(EZPlaylist playlist, SongInfo song)
        {
            Songs = playlist.Songs;
            updateSongInfo(song);
            _media_player.Play(playlist, song);
        }

        /// <summary>
        /// Call this when you change the current song to the next or previous song in the collection.
        /// </summary>
        /// <param name="song"></param>
        private void updateSongInfo(SongInfo song)
        {
            CurrentSongInfo = song;
            AlbumArt = findAlbumArt(song);
            _currentTimeOfSong = TimeSpan.FromSeconds(0);
        }

        private BitmapImage findAlbumArt(SongInfo song)
        {
            BitmapImage image;
            if (song != null && _library.Albums.Count != 0)
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void _media_player_CurrentMediaChanged(object sender, MediaChangedEventArgs e)
        {
            updateSongInfo(e.SongNowPlaying);
            _timer.Start();
            
        }

        private void _media_player_MediaStateChanged(object sender, MediaStateEventArgs e)
        {
            if (e.SongMediaState == MediaState.Playing)
            {
                _timer.Start();
                PlaySongPictureSource = "/Assets/MusicButtons/PauseButton.png";
            }
            else
            {
                _timer.Stop();
                PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _currentTimeOfSong = _currentTimeOfSong + TimeSpan.FromSeconds(1);
            SongTime = _currentTimeOfSong.ToString();
        }
    }
}
