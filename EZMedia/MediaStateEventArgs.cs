using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia
{
    public class MediaStateEventArgs : EventArgs
    {
        private readonly SongInfo _currentSong;
        private readonly MediaState _songMediaState;
        private readonly TimeSpan _playPosition;

        public MediaStateEventArgs(SongInfo currentSong, MediaState state, TimeSpan playPosition)
        {
            _currentSong = currentSong;
            _songMediaState = state;
            _playPosition = playPosition;
        }

        public SongInfo CurrentSong
        {
            get
            {
                return _currentSong;
            }
        }

        public MediaState SongMediaState
        {
            get
            {
                return _songMediaState;
            }
        }

        public TimeSpan PlayPosition
        {
            get
            {
                return _playPosition;
            }
        }
    }
}
