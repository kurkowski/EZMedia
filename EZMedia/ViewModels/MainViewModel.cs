using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EZMedia.Resources;
using EZMedia.ViewModels;

namespace EZMedia.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            _library = new MediaLibrary();
            this.Songs = new ObservableCollection<SongInfo>();
            this.Albums = new ObservableCollection<AlbumInfo>();
            this.Artists = new ObservableCollection<ArtistInfo>();
            this.Playlists = new ObservableCollection<EZPlaylist>();
            this.SongPlayingVM = new SongPlayingViewModel(new SongInfo());
        }

        /// <summary>
        /// A collection for SongInfo objects.
        /// </summary>
        public ObservableCollection<SongInfo> Songs { get; private set; }
        public ObservableCollection<AlbumInfo> Albums { get; private set; }
        public ObservableCollection<ArtistInfo> Artists { get; private set; }
        public ObservableCollection<EZPlaylist> Playlists { get; private set; }

        public SongPlayingViewModel SongPlayingVM { get; set; }

        private MediaLibrary _library;

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few SongInfo objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            foreach (Song song in _library.Songs)
            {
                this.Songs.Add(createSongInfoFromSong(song));
            }

            foreach (Album album in _library.Albums)
            {
                this.Albums.Add(createAlbumInfoFromAlbum(album));
            }

            foreach (Artist artist in _library.Artists)
            {
                this.Artists.Add(createArtistInfoFromArtist(artist));
            }

            this.IsDataLoaded = true;
        }

        private SongInfo createSongInfoFromSong(Song song)
        {
            SongInfo si = new SongInfo(
                song.Artist.Name,
                song.Album.Name,
                song.Name,
                song.Duration,
                song.TrackNumber);
            return si;
        }

        private AlbumInfo createAlbumInfoFromAlbum(Album album)
        {
            return new AlbumInfo(album.Artist.Name, album.Name);
        }

        private ArtistInfo createArtistInfoFromArtist(Artist artist)
        {
            return new ArtistInfo(artist.Name);
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