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
            // Timer to run the XNA internals (MediaPlayer is from XNA)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            library = new MediaLibrary();
            songs = new List<SongInfo>();
            PlayCommand = new PlayPauseCommand(null, this);

            ShuffleSongPictureSource = "/Assets/MusicButtons/ShuffleSongsNotClicked.png";
            RepeatSongPictureSource = "/Assets/MusicButtons/RepeatSongsNotClicked.png";
            UpdateNowPlayingSong(new List<SongInfo>().AsReadOnly(), null);
        }

        private MediaLibrary library;

        private ICommand playCommand;
        public ICommand PlayCommand 
        {
            get
            {
                return playCommand;
            }
            private set
            {
                playCommand = value;
                NotifyPropertyChanged("PlayCommand");
            }
        }
        public ICommand PreviousSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand ShuffleSongsCommand { get; private set; }
        public ICommand RepeatSongsCommand { get; private set; }

        private string playSongPictureSource;
        public string PlaySongPictureSource
        {
            get
            {
                return playSongPictureSource;
            }
            set
            {
                playSongPictureSource = value;
                NotifyPropertyChanged("PlaySongPictureSource");
            }
        }

        private string shuffleSongPictureSource;
        public string ShuffleSongPictureSource
        {
            get
            {
                return shuffleSongPictureSource;
            }
            set
            {
                shuffleSongPictureSource = value;
                NotifyPropertyChanged("ShuffleSongPictureSource");
            }
        }

        private string repeatSongPictureSource;
        public string RepeatSongPictureSource
        {
            get
            {
                return repeatSongPictureSource;
            }
            set
            {
                repeatSongPictureSource = value;
                NotifyPropertyChanged("RepeatSongPictureSource");
            }
        }

        private List<SongInfo> songs;
        public ReadOnlyCollection<SongInfo> Songs
        {
            get
            {
                return songs.AsReadOnly();
            }
            private set
            {
                if (value != null)
                {
                    songs.Clear();
                    songs.AddRange(value.ToList());
                    NotifyPropertyChanged("Songs");
                }
            }
        }
        public SongInfo CurrentSongInfo { get; private set; }
        public BitmapImage AlbumArt { get; private set; }

        /// <summary>
        /// Method for passing along a song to play and a list of songs to be played after that.
        /// The "song" to play should also be contained in "songsToPlay"
        /// </summary>
        /// <param name="songsToPlay">List of songs to play</param>
        /// <param name="songInfo">The currently selected song to play. This serves as the start point in the collection "songsToPlay". If it's null,
        /// no song will play.</param>
        public void UpdateNowPlayingSong(ReadOnlyCollection<SongInfo> songsToPlay, SongInfo song)
        {
            CurrentSongInfo = song;
            Songs = songsToPlay;
            AlbumArt = findAlbumArt(song);
            playSong();
        }

        /// <summary>
        /// Plays the song that matches the CurrentSongInfo
        /// </summary>
        private void playSong()
        {
            if (CurrentSongInfo != null)
            {
                PlaySongPictureSource = "/Assets/MusicButtons/PauseButton.png";
                reorderSongsWithCurrentSongFirst();
                PlayCommand = new PlayPauseCommand(findSongFromSongInfo(), this);
            }
            else
            {
                PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
            }
        }

        

        /// <summary>
        /// Checks the CurrentSongInfo for a match in the media library. When a match is found, it
        /// returns the song.
        /// </summary>
        /// <returns></returns>
        private Song findSongFromSongInfo()
        {
            foreach (Song song in library.Songs)
            {
                if (checkIfSongInfoIsSameAsSong(song))
                {
                    return song;
                }
            }
            return null;
        }

        /// <summary>
        /// returns true if the Song object contains the same information as the
        /// CurrentSongInfo
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        private bool checkIfSongInfoIsSameAsSong(Song song)
        {
            if (song.Album.Name == CurrentSongInfo.Album &&
                song.Artist.Name == CurrentSongInfo.Artist &&
                song.Duration == CurrentSongInfo.Duration &&
                song.Name == CurrentSongInfo.Name &&
                song.TrackNumber == CurrentSongInfo.TrackNumber)
                return true;
            else
                return false;
        }

        private BitmapImage findAlbumArt(SongInfo song)
        {
            BitmapImage image;
            if (song != null)
            {
                Album album = library.Albums.Where(x => x.Name == song.Album).ToList()[0];
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

        private void reorderSongsWithCurrentSongFirst()
        {
            int index = 0; //index of CurrentSongInfo
            List<SongInfo> tempList = new List<SongInfo>();

            //traverse list until CurrentSongInfo is found
            foreach(SongInfo song in Songs)
            {
                if (song == CurrentSongInfo)
                {
                    break;
                }
                index++;
            }
            //you don't need to reorder the list if CurrentSongInfo is the very first song in the list
            if (index > 0)
            {
                //make a temporary list in the interval [CurrentSongInfo, last element of Songs]
                for (int i = index; i < Songs.Count; i++)
                {
                    tempList.Add(Songs[i]);
                }

                //make another temp list in the interval [0, CurrentSongInfo-1]
                List<SongInfo> tempList2 = new List<SongInfo>();
                for (int j = 0; j < index; j++)
                {
                    tempList2.Add(Songs[j]);
                }

                //merge the lists
                Songs = tempList.Concat(tempList2).ToList().AsReadOnly();
                tempList.Clear();
                tempList2.Clear();
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
