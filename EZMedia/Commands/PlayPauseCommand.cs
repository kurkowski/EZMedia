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
        public PlayPauseCommand()
        {
            FrameworkDispatcher.Update();
            //MediaPlayer.Play();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
                
            }
            else if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }
    }
}
