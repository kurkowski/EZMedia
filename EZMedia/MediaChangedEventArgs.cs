using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia
{
    public class MediaChangedEventArgs : EventArgs
    {
        private readonly SongInfo _songNowPlaying;
        private readonly int _index;

        public MediaChangedEventArgs(SongInfo song, int index)
        {
            _songNowPlaying = song;
            _index = index;
        }

        public SongInfo SongNowPlaying
        {
            get
            {
                return _songNowPlaying;
            }
        }

        public int Index
        {
            get
            {
                return _index;
            }
        }
    }
}
