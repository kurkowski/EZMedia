using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EZMedia.Commands
{
    public class NextSongCommand : ICommand
    {
        private EZMediaPlayer _mediaPlayer;
        public NextSongCommand(EZMediaPlayer mediaPlayer)
        {
            _mediaPlayer = mediaPlayer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_mediaPlayer.Songs != null)
            {
                if (_mediaPlayer.Songs.Count > 0)
                {
                    _mediaPlayer.Next();
                }
            }
        }
    }
}
