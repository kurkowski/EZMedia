using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;

namespace EZMedia
{
    public class SongInfo :IEquatable<SongInfo>
    {
        public string Artist
        {
            get;
            set;
        }
        public string Album
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

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
            Artist = artist;
            Album = album;
            Name = name;
            Duration = duration;
            TrackNumber = trackNumber;
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
                if (Artist == si.Artist && Album == si.Album && Name == si.Name && Duration == si.Duration && TrackNumber == si.TrackNumber)
                    return true;
                else
                    return false;
            }
        }

        public bool Equals(SongInfo SongInfo)
        {
            return Equals((object)SongInfo);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 23 + Name.GetHashCode();
            hash *= 23 + TrackNumber.GetHashCode();
            hash *= 23 + Duration.GetHashCode();
            hash *= 23 + Album.GetHashCode();
            hash *= 23 + Artist.GetHashCode();

            return hash;
        }

    }

    public class AlbumInfo
    {
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public ReadOnlyCollection<SongInfo> Songs { get; private set; }
        public BitmapImage AlbumArt { get; set; }

        public AlbumInfo(string artistName, string albumName, ReadOnlyCollection<SongInfo> songs, BitmapImage image)
        {
            ArtistName = artistName;
            AlbumName = albumName;
            Songs = songs;
            AlbumArt = image;
        }
    }

    public class ArtistInfo
    {
        public string ArtistName { get; set; }
        public ReadOnlyCollection<AlbumInfo> Albums { get; private set; }

        public ArtistInfo(string artistName, ReadOnlyCollection<AlbumInfo> albums)
        {
            ArtistName = artistName;
            Albums = albums;
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
            set { _name = value; }
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
            Name = "";
        }

        public EZPlaylist(string name)
        {
            _playlist = new List<SongInfo>();
            Name = sanitizeString(name);
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
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(Name + ".xml", System.IO.FileMode.Create))
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

        public void ReadFromFile(string playlistName)
        {
            _playlist.Clear();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(playlistName + ".xml", System.IO.FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SongInfo>));
                        _playlist = (List<SongInfo>)serializer.Deserialize(stream);
                        Name = playlistName;
                    }
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
        /// Deletes the playlist in memory
        /// </summary>
        public void Clear()
        {
            _playlist.Clear();
        }

        /// <summary>
        /// Deletes the stored playlist file
        /// </summary>
        public void DeletePlaylist()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    myIsolatedStorage.DeleteFile(Name + ".xml");
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
