using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EZMedia.Commands;
using System.Windows.Threading;

namespace EZMedia
{
    public class EZMediaPlayer
    {
        private MediaLibrary _library;
        private bool _isPlaylist;
        private bool _newSongListPlaying;

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
        private Song _currentSong;

        public EZMediaPlayer()
        {
            // Timer to run the XNA internals (MediaPlayer is from XNA)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            _library = new MediaLibrary();
            _isPlaylist = false;
            IsShuffled = false;
            IsRepeating = false;
            IsRepeatingOne = false;
            _songs = new List<SongInfo>();

            MediaPlayer.ActiveSongChanged += MediaPlayer_ActiveSongChanged;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private bool _isShuffled;
        public bool IsShuffled
        {
            get
            {
                return _isShuffled;
            }
            set
            {
                if (!_isPlaylist)
                {
                    MediaPlayer.IsShuffled = value;
                }
                _isShuffled = value;
            }
        }

        private bool _isRepeating;
        public bool IsRepeating
        {
            get
            {
                return _isRepeating;
            }
            set
            {
                if (!_isPlaylist)
                {
                    MediaPlayer.IsRepeating = value;
                }
                _isRepeating = value;
            }
        }

        private bool _isRepeatingOne;
        public bool IsRepeatingOne
        {
            get
            {
                return _isRepeatingOne;
            }
            set
            {
                _isRepeatingOne = value;
            }
        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            private set
            {
                if (value >= Songs.Count)
                {
                    _index = 0;
                }
                else if (value < 0)
                {
                    _index = Songs.Count - 1;
                }
                else
                {
                    _index = value;
                }
            }
        }

        /// <summary>
        /// Plays the playlist starting with songToPlay.
        /// </summary>
        /// <param name="playlist"></param>
        public void Play(EZPlaylist playlist, SongInfo songToPlay)
        {
            _isPlaylist = true;
            _newSongListPlaying = true;
            _currentSong = findSong(songToPlay);
            Songs = playlist.Songs;
            Index = findIndexOfSongInCollection(songToPlay);
            FrameworkDispatcher.Update();
            MediaPlayer.Play(_currentSong);
        }

        /// <summary>
        /// Plays a collection of songs that represent all the songs in the library.
        /// </summary>
        /// <param name="songs"></param>
        /// <param name="songToPlay"></param>
        public void Play(ReadOnlyCollection<SongInfo> songs, SongInfo songToPlay)
        {
            _isPlaylist = false;
            _newSongListPlaying = true;
            Songs = songs;
            Index = findIndexOfSongInCollection(songToPlay);
            FrameworkDispatcher.Update();
            MediaPlayer.Play(_library.Songs, Index);
        }

        /// <summary>
        /// Plays the album starting with songToPlay
        /// </summary>
        /// <param name="album"></param>
        /// <param name="songToPlay"></param>
        public void Play(AlbumInfo album, SongInfo songToPlay)
        {
            _isPlaylist = false;
            _newSongListPlaying = true;
            Songs = album.Songs;
            Index = findIndexOfSongInCollection(songToPlay);
            Album albumToPlay = findAlbum(album);
            FrameworkDispatcher.Update();
            MediaPlayer.Play(albumToPlay.Songs, Index);
        }

        /// <summary>
        /// Plays songs by a specified artist starting with songToPlay
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="songToPlay"></param>
        public void Play(ArtistInfo artist, SongInfo songToPlay)
        {
            _isPlaylist = false;
            _newSongListPlaying = true;
            Songs = artist.Songs;
            Index = findIndexOfSongInCollection(songToPlay);
            Artist artistToPlay = findArtist(artist);
            FrameworkDispatcher.Update();
            MediaPlayer.Play(artistToPlay.Songs, Index);
        }

        /// <summary>
        /// Make sure the Songs property is set before using this helper.
        /// </summary>
        /// <param name="songToPlay"></param>
        /// <returns></returns>
        private int findIndexOfSongInCollection(SongInfo songToPlay)
        {
            int i = 0;
            foreach (SongInfo si in Songs)
            {
                if (si.Equals(songToPlay))
                {
                    break;
                }
                i++;
            }
            return i;
        }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }
        public void Next()
        {
            updateCurrentSongInfo(1);
            
            if (_isPlaylist)
            {
                _currentSong = findSong(CurrentSongInfo);
                FrameworkDispatcher.Update();
                MediaPlayer.Play(_currentSong);
            }
            else
            {
                MediaPlayer.MoveNext();
            }
        }
        public void Previous()
        {
            updateCurrentSongInfo(-1);

            if (_isPlaylist)
            {
                _currentSong = findSong(CurrentSongInfo);
                FrameworkDispatcher.Update();
                MediaPlayer.Play(_currentSong);
            }
            else
            {
                MediaPlayer.MovePrevious();
            }
        }

        private Song findSong(SongInfo songInfo)
        {
            foreach (Song song in _library.Songs)
            {
                if (songInfo.CheckIfEqualToSong(song))
                {
                    return song;
                }
            }
            return null;
        }

        private Artist findArtist(ArtistInfo artistInfo)
        {
            foreach (Artist artist in _library.Artists)
            {
                if(artistInfo.CheckIfEqualToArtist(artist))
                {
                    return artist;
                }
            }
            return null;
        }

        private Album findAlbum(AlbumInfo albumInfo)
        {
            foreach (Album album in _library.Albums)
            {
                if (albumInfo.CheckIfEqualToAlbum(album))
                {
                    return album;
                }
            }
            return null;
        }

        private void updateCurrentSongInfo(int indexOffset)
        {
            Index += indexOffset;
            CurrentSongInfo = Songs[Index];
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            
            if (MediaPlayer.State == MediaState.Stopped)
            {
                Next();
            }
            else
            {
                OnMediaStateChanged(new MediaStateEventArgs(CurrentSongInfo, MediaPlayer.State, MediaPlayer.PlayPosition));
            }
        }

        /// <summary>
        /// This function is called when a song is chosen as well as when the song changes in next or previous.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_ActiveSongChanged(object sender, EventArgs e)
        {
            if (_newSongListPlaying)
            {
                _newSongListPlaying = false;
                updateCurrentSongInfo(0);
            }
            else if (!_isPlaylist)
            {
                MediaQueue queue = MediaPlayer.Queue;
                SongInfo song = new SongInfo(queue.ActiveSong);
                if(song.Equals(Songs[indexAdd(-1)]))
                {
                    updateCurrentSongInfo(-1);
                }
                else if(song.Equals(Songs[indexAdd(1)]))
                {
                    updateCurrentSongInfo(1);
                }
                
            }
            OnCurrentMediaChanged(new MediaChangedEventArgs(CurrentSongInfo));
        }

        private int indexAdd(int offset)
        {
            int n = Index + offset;
            int m = Songs.Count;

            return (n % m + m) % m;
        }

        public event EventHandler<MediaChangedEventArgs> CurrentMediaChanged;
        public event EventHandler<MediaStateEventArgs> MediaStateChanged;

        private void OnCurrentMediaChanged(MediaChangedEventArgs e)
        {
            EventHandler<MediaChangedEventArgs> handler = CurrentMediaChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnMediaStateChanged(MediaStateEventArgs e)
        {
            EventHandler<MediaStateEventArgs> handler = MediaStateChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
