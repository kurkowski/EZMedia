using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EZMedia.ViewModels;
using System.Windows.Media.Imaging;
using Microsoft.Phone.BackgroundAudio;

namespace EZMedia.Commands
{
    public class PlayPauseCommand : ICommand
    {
        private EZMediaPlayer _mediaPlayer;
        public PlayPauseCommand(EZMediaPlayer mediaPlayer)
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
            if (MediaPlayer.State == MediaState.Playing)
            {
                _mediaPlayer.Pause();
            }
            else
            {
                _mediaPlayer.Resume();
            }
        }
    }
}
