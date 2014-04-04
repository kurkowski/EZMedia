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

        public EZMediaPlayer()
        {
            // Timer to run the XNA internals (MediaPlayer is from XNA)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            _library = new MediaLibrary();
        }

        /// <summary>
        /// Plays the playlist starting with songToPlay.
        /// </summary>
        /// <param name="playlist"></param>
        public void Play(EZPlaylist playlist, SongInfo songToPlay)
        {

        }

        /// <summary>
        /// Plays a collection of songs (eg. all the songs in the library) starting with songToPlay
        /// </summary>
        /// <param name="songs"></param>
        /// <param name="songToPlay"></param>
        public void Play(ReadOnlyCollection<SongInfo> songs, SongInfo songToPlay)
        {
            
        }

        /// <summary>
        /// Plays the album starting with songToPlay
        /// </summary>
        /// <param name="album"></param>
        /// <param name="songToPlay"></param>
        public void Play(AlbumInfo album, SongInfo songToPlay)
        {
            
        }

        /// <summary>
        /// Plays songs by a specified artist starting with songToPlay
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="songToPlay"></param>
        public void Play(ArtistInfo artist, SongInfo songToPlay)
        {
            
        }

        private int findIndexOfSongInCollection(ReadOnlyCollection<SongInfo> songs, SongInfo songToPlay)
        {
            int i = 0;
            foreach (SongInfo si in songs)
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
            
        }

        public void Pause()
        {

        }
        public void Next()
        {

        }
        public void Previous()
        {

        }
        public void Shuffle()
        {

        }
        public void Repeat()
        {

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

        public event EventHandler<EventArgs> CurrentMediaChanged;
        public event EventHandler<EventArgs> MediaStateChanged;
    }
}
