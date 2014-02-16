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

namespace EZMedia.ViewModels
{
    public class SongPlayingViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for passing along a song to play and a list of songs to be played after that.
        /// The "song" to play should also be contained in "songsToPlay"
        /// </summary>
        /// <param name="songsToPlay">List of songs to play</param>
        /// <param name="songInfo">The currently selected song to play. This serves as the start point in the collection "songsToPlay".</param>
        public SongPlayingViewModel(ReadOnlyCollection<SongInfo> songsToPlay, SongInfo song)
        {
            CurrentSongInfo = song;
            Songs = songsToPlay;
            AlbumArt = findAlbumArt(song);
            reorderSongsWithCurrentSongFirst();
        }

        public ReadOnlyCollection<SongInfo> Songs { get; private set; }
        public SongInfo CurrentSongInfo { get; private set; }
        public BitmapImage AlbumArt { get; private set; }

        private BitmapImage findAlbumArt(SongInfo song)
        {
            MediaLibrary library = new MediaLibrary();
            BitmapImage image;

            Album album = library.Albums.Where(x => x.Name == song.Album).ToList()[0];
            if (album.HasArt)
            {
                image = new BitmapImage();
                image.SetSource(album.GetAlbumArt());
            }
            else
            {
                image = new BitmapImage(new Uri("/Assets/ApplicationIcon.png", UriKind.Relative));
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
