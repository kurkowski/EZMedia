using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace EZMedia
{
    public interface IMediaInfo
    {
        string Name { get; set; }
        string ArtistName { get; set; }
    }

    public class SongInfo : IMediaInfo, IEquatable<SongInfo>
    {
        public string ArtistName
        {
            get;
            set;
        }
        public string Album
        {
            get;
            set;
        }
        public string Name { get; set; }
        private TimeSpan _duration;

        [XmlIgnore]
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Used primarily for serialization. TimeSpan cannot be serialized properly
        /// </summary>
        public long DurationTicks
        {
            get { return _duration.Ticks; }
            set { _duration = new TimeSpan(value); }
        }
        public int TrackNumber
        {
            get;
            set;
        }

        public SongInfo()
        {

        }
        public SongInfo(
            string artist,
            string album,
            string name,
            TimeSpan duration,
            int trackNumber)
        {
            ArtistName = artist;
            Album = album;
            Name = name;
            Duration = duration;
            TrackNumber = trackNumber;
        }

        public SongInfo(Song song)
        {
            ArtistName = song.Artist.Name;
            Album = song.Album.Name;
            Name = song.Name;
            Duration = song.Duration;
            TrackNumber = song.TrackNumber;
        }

        public bool CheckIfEqualToSong(Song song)
        {
            if (song.Album.Name == Album &&
                song.Artist.Name == ArtistName &&
                song.Duration == Duration &&
                song.Name == Name &&
                song.TrackNumber == TrackNumber)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
            { 
                return false; 
            }

            SongInfo si = obj as SongInfo;
            if ((object)si == null)
            {
                return false;
            }
            else
            {
                if (ArtistName == si.ArtistName && Album == si.Album && Name == si.Name && Duration == si.Duration && TrackNumber == si.TrackNumber)
                    return true;
                else
                    return false;
            }
        }

        public bool Equals(SongInfo songInfo)
        {
            return Equals((object)songInfo);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 23 + Name.GetHashCode();
            hash *= 23 + TrackNumber.GetHashCode();
            hash *= 23 + Duration.GetHashCode();
            hash *= 23 + Album.GetHashCode();
            hash *= 23 + ArtistName.GetHashCode();

            return hash;
        }
    }

    public class AlbumInfo : IMediaInfo, IEquatable<AlbumInfo>
    {
        public string ArtistName { get; set; }
        public string Name { get; set; }
        public ReadOnlyCollection<SongInfo> Songs { get; private set; }
        public BitmapImage AlbumArt { get; private set; }

        public AlbumInfo(string artistName, string albumName, ReadOnlyCollection<SongInfo> songs)
        {
            ArtistName = artistName;
            Name = albumName;
            Songs = songs;
        }

        public AlbumInfo(Album album)
        {
            
            ArtistName = album.Artist.Name;
            Name = album.Name;

            List<SongInfo> albumSongs = new List<SongInfo>();
            foreach (Song song in album.Songs)
            {
                albumSongs.Add(new SongInfo(song));
            }
            Songs = albumSongs.AsReadOnly();

            if (album.HasArt)
            {
                AlbumArt = new BitmapImage();
                AlbumArt.SetSource(album.GetAlbumArt());
            }
            else
            {
                try
                {
                    AlbumArt = new BitmapImage(new Uri("/Assets/NoAlbumArt.png", UriKind.Relative));
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
        }

        public bool CheckIfEqualToAlbum(Album album)
        {
            if (album.Name == Name &&
                album.Artist.Name == ArtistName &&
                checkIfSongsAreSame(album.Songs))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AlbumInfo ai = obj as AlbumInfo;
            if ((object)ai == null)
            {
                return false;
            }
            else
            {
                if (ArtistName == ai.ArtistName && Name == ai.Name && checkIfSongsAreSame(ai.Songs))
                    return true;
                else
                    return false;
            }
        }

        private bool checkIfSongsAreSame(SongCollection songs)
        {
            int i = 0;
            foreach (Song song in songs)
            {
                if (!Songs[i].CheckIfEqualToSong(song))
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        private bool checkIfSongsAreSame(ReadOnlyCollection<SongInfo> songs)
        {
            if (Songs == null && songs == null)
                return true;
            if ((songs == null && Songs != null) || (songs != null && Songs == null))
                return false;
            if (Songs.Count != songs.Count)
                return false;
            int index = 0;
            foreach (SongInfo si in Songs)
            {
                if (si != songs[index])
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        public bool Equals(AlbumInfo albumInfo)
        {
            return Equals((object)albumInfo);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 23 + Name.GetHashCode();
            hash *= 23 + ArtistName.GetHashCode();
            hash *= 23 + Songs.GetHashCode();

            return hash;
        }
    }

    public class ArtistInfo : IMediaInfo, IEquatable<ArtistInfo>
    {
        public string Name { get; set; }
        public string ArtistName { get; set; }
        public ReadOnlyCollection<AlbumInfo> Albums { get; private set; }
        public ReadOnlyCollection<SongInfo> Songs { get; private set; }

        public ArtistInfo(Artist artist)
        {
            Name = artist.Name;
            ArtistName = artist.Name;
            setAlbums(artist);
            setSongs(artist);
        }

        private void setSongs(Artist artist)
        {
            List<SongInfo> tempSongs = new List<SongInfo>();
            foreach (Song song in artist.Songs)
            {
                tempSongs.Add(new SongInfo(song));
            }
            Songs = tempSongs.AsReadOnly();
        }

        private void setAlbums(Artist artist)
        {
            List<AlbumInfo> tempAlbums = new List<AlbumInfo>();
            foreach (Album album in artist.Albums)
            {
                tempAlbums.Add(new AlbumInfo(album));
            }
            Albums = tempAlbums.AsReadOnly();
        }

        public bool CheckIfEqualToArtist(Artist artist)
        {
            if (artist.Name == Name &&
            checkIfAlbumsAreSame(artist.Albums) &&
            checkIfSongsAreSame(artist.Songs))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkIfAlbumsAreSame(AlbumCollection albums)
        {
            int i = 0;
            foreach (Album album in albums)
            {
                if (!Albums[i].CheckIfEqualToAlbum(album))
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        private bool checkIfSongsAreSame(SongCollection songs)
        {
            int i = 0;
            foreach (Song song in songs)
            {
                if (!Songs[i].CheckIfEqualToSong(song))
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ArtistInfo ai = obj as ArtistInfo;
            if ((object)ai == null)
            {
                return false;
            }
            else
            {
                if (Name == ai.Name && checkIfAlbumsAreSame(ai.Albums))
                    return true;
                else
                    return false;
            }
        }



        private bool checkIfAlbumsAreSame(ReadOnlyCollection<AlbumInfo> albums)
        {
            if (Albums == null && albums == null)
                return true;
            if ((albums == null && Albums != null) || (albums != null && Albums == null))
                return false;
            if (Albums.Count != albums.Count)
                return false;
            int index = 0;
            foreach (AlbumInfo ai in Albums)
            {
                if (ai != albums[index])
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        public bool Equals(ArtistInfo artistInfo)
        {
            return Equals((object)artistInfo);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 23 + Name.GetHashCode();
            hash *= 23 + Albums.GetHashCode();

            return hash;
        }
    }

    /// <summary>
    /// V1.0 Represents a playlist of songs. Songs can be arranged by Artist, Album,
    /// Alphabetically, or Custom order
    /// </summary>
    public class EZPlaylist : IList<SongInfo>
    {
        protected const int _maxPlaylistNameLength = 25;
        protected List<SongInfo> _playlist;
        string _name;
        public string Name
        {
            get { return _name; }
        }

        public ReadOnlyCollection<SongInfo> Songs
        {
            get
            {
                return _playlist.AsReadOnly();
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public EZPlaylist()
        {
            _playlist = new List<SongInfo>();
            _name = "";
        }

        public EZPlaylist(string name)
        {
            _playlist = new List<SongInfo>();
            _name = sanitizeString(name);
        }

        public void CopyTo(SongInfo[] array, int index)
        {
            _playlist.CopyTo(array, index);
        }

        public IEnumerator<SongInfo> GetEnumerator()
        {
            return _playlist.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(SongInfo elem)
        {
            _playlist.Add(elem);
        }

        public bool Remove(SongInfo elem)
        {
            return _playlist.Remove(elem);
        }

        public void RemoveAt(int index)
        {
            _playlist.RemoveAt(index);
        }

        public bool Contains(SongInfo song)
        {
            return _playlist.Contains(song);
        }

        public SongInfo this[int index]
        {
            get { return _playlist[index]; }
            set { _playlist[index] = value; }
        }

        public void Insert(int index, SongInfo song)
        {
            _playlist.Insert(index, song);
        }

        public int IndexOf(SongInfo song)
        {
            return _playlist.IndexOf(song);
        }

        public void AddList(IReadOnlyCollection<SongInfo> list)
        {
            _playlist.AddRange(list);
        }

        /// <summary>
        /// If the playlists name is "X", the playlist will be saved to an .xml file called "X.xml".
        /// </summary>
        public void SavePlaylist()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("EZ" + Name + ".xml", System.IO.FileMode.Create))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SongInfo>));
                        serializer.Serialize(stream, _playlist);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// This fills the object with the songs from the playlist called playlistName from disk. Any
        /// songs added to the playlist before this method is called will be removed.
        /// </summary>
        /// <param name="playlistName">The name of the playlist that you want to get the Song info from.</param>
        public void ReadFromFile(string playlistName)
        {
            _playlist.Clear();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("EZ" + playlistName + ".xml", System.IO.FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SongInfo>));
                        _playlist = (List<SongInfo>)serializer.Deserialize(stream);
                        _name = playlistName;
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                e.ToString();
            }
        }

        public void RenamePlaylist(string newPlaylistName)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string oldName = Name;
                    _name = sanitizeString(newPlaylistName);
                    myIsolatedStorage.MoveFile("EZ" + oldName + ".xml", "EZ" + _name + ".xml");
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                e.ToString();
            }
        }

        ///
        private string sanitizeString(string playlistName)
        {
            string temp;
            if (playlistName.Length > _maxPlaylistNameLength)
                temp = playlistName.Substring(0, _maxPlaylistNameLength);
            else
                temp = playlistName;

            StringBuilder sb = new StringBuilder(_maxPlaylistNameLength);
            foreach (char c in temp)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '_' || c == ' ' || c == '(' || c == ')' || c == '!'
                    || c == '@' || c == '#' || c == '$' || c == '%' || c == '&' || c == '-' || c == ',' || c == '=' || c == '+')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Deletes the playlist in memory. Does NOT delete playlist on disk. Call DeletePlaylist() to delete
        /// playlist on disk.
        /// </summary>
        public void Clear()
        {
            _playlist.Clear();
        }

        //Deletes the playlist in memory and on disk.
        public void DeletePlaylist()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    myIsolatedStorage.DeleteFile("EZ" + Name + ".xml");
                    _playlist.Clear();
                }
                catch { }
            }
        }

        public int Count
        {
            get
            {
                return _playlist.Count;
            }
        }
    }
}
