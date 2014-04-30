using EZMedia.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EZMedia.ViewModels
{
    public class ItemWithCheckBox : INotifyPropertyChanged
    {
        public ItemWithCheckBox(bool isChecked, IMediaInfo media)
        {
            IsChecked = isChecked;
            Media = media;
        }
        private bool _isChecked;
        public bool IsChecked 
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
        public IMediaInfo Media { get; private set; }

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

    public class PlaylistsViewModel : INotifyPropertyChanged
    {
        public PlaylistsViewModel()
        {
            _playlists = new List<ItemWithCheckBox>();
            findPlaylistsInStorage();
        }

        private List<ItemWithCheckBox> _playlists;
        public IReadOnlyCollection<ItemWithCheckBox> Playlists
        {
            get
            {
                return _playlists;
            }
            set
            {
                _playlists.Clear();
                _playlists.AddRange(value);
                NotifyPropertyChanged("Playlists");
            }
        }

        public void AddNewPlaylist(string name)
        {
            EZPlaylist playlist = new EZPlaylist(name);
            playlist.SavePlaylist();
            _playlists.Add(new ItemWithCheckBox(false, playlist));
            NotifyPropertyChanged("Playlists");
        }

        private List<ItemWithCheckBox> toBeRemoved = new List<ItemWithCheckBox>();

        /// <summary>
        /// Deletes playlists that are checked.
        /// </summary>
        public void RemovePlaylists()
        {
            bool playlistChanged = false;

            toBeRemoved.Clear();

            foreach (ItemWithCheckBox item in _playlists)
            {
                if (item.IsChecked)
                {
                    
                    playlistChanged = true;
                    EZPlaylist pl = item.Media as EZPlaylist;
                    toBeRemoved.Add(item);
                    pl.DeletePlaylist();
                }
            }

            if (playlistChanged)
            {
                foreach (ItemWithCheckBox item in toBeRemoved)
                {
                    _playlists.Remove(item);
                }
                toBeRemoved.Clear();
                NotifyPropertyChanged("Playlists");
            }
        }

        private void findPlaylistsInStorage()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string[] strArray = myIsolatedStorage.GetFileNames();
                List<ItemWithCheckBox> playlistNames = new List<ItemWithCheckBox>();
                
                foreach (string str in strArray)
                {
                    if (str.Substring(0, 2) == "EZ" && str.Substring(str.Length - 4, 4) == ".xml")
                    {
                        string name = str.Substring(2, str.Length - 6);
                        EZPlaylist playlist = new EZPlaylist();
                        playlist.ReadFromFile(name);
                        playlistNames.Add(new ItemWithCheckBox(false, playlist));
                    }
                }
                Playlists = playlistNames;
            }
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
