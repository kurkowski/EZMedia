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

            //subscribe to events
            _media_player.CurrentMediaChanged += _media_player_CurrentMediaChanged;
            _media_player.MediaStateChanged += _media_player_MediaStateChanged;
            _media_player.TimerIntervalReached += _media_player_TimerIntervalReached;
            _media_player.SongOrderChanged += _media_player_SongOrderChanged;

            //initialize properties
            PlayCommand = new PlayPauseCommand(_media_player);
            NextSongCommand = new NextSongCommand(_media_player);
            PreviousSongCommand = new PreviousSongCommand(_media_player);
            ShuffleSongsCommand = new ShuffleSongsCommand(_media_player, this);
            RepeatSongsCommand = new RepeatSongsCommand(_media_player, this);

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

        private ICommand _shuffleSongsCommand;
        public ICommand ShuffleSongsCommand 
        {
            get
            {
                return _shuffleSongsCommand;
            }
            private set
            {
                _shuffleSongsCommand = value;
                NotifyPropertyChanged("ShuffleSongsCommand");
            }
        }

        private ICommand _repeatSongsCommand;
        public ICommand RepeatSongsCommand 
        {
            get
            {
                return _repeatSongsCommand;
            }
            private set
            {
                _repeatSongsCommand = value;
                NotifyPropertyChanged("RepeatSongsCommand");
            }
        }

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

        private ReadOnlyCollection<SongInfo> _songs;
        public ReadOnlyCollection<SongInfo> Songs
        {
            get
            {
                return _songs;
            }
            private set
            {
                _songs = value;
                NotifyPropertyChanged("Songs");
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

        private TimeSpan _currentTimeOfSong;

        private int _index;
        public int CurrentSongIndex
        {
            get { return _index; }
            set 
            { 
                _index = value;
                NotifyPropertyChanged("CurrentSongIndex");
            }
        }

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
        /// make sure Songs is set before you use this method
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        private int findIndex(SongInfo song)
        {
            int i = 0;
            foreach (SongInfo si in Songs)
            {
                if (si.Equals(song))
                {
                    break;
                }
                i++;
            }
            return i;
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
            SongTime = _currentTimeOfSong.ToString();
            CurrentSongIndex = findIndex(song);
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
            CurrentSongIndex = e.Index;
        }

        private void _media_player_MediaStateChanged(object sender, MediaStateEventArgs e)
        {
            if (e.SongMediaState == MediaState.Playing)
            {
                PlaySongPictureSource = "/Assets/MusicButtons/PauseButton.png";
            }
            else
            {
                PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
            }
        }

        private void _media_player_TimerIntervalReached(object sender, MediaStateEventArgs e)
        {
            SongTime = e.PlayPosition.ToString(@"hh\:mm\:ss");
        }

        private void _media_player_SongOrderChanged(object sender, EventArgs e)
        {
            Songs = _media_player.Songs;
        }
    }
}
