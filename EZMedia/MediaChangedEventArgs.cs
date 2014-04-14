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

        public MediaChangedEventArgs(SongInfo song)
        {
            _songNowPlaying = song;
        }

        public SongInfo SongNowPlaying
        {
            get
            {
                return _songNowPlaying;
            }
        }
    }
}
