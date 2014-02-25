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

namespace EZMedia.Commands
{
    public class PlayPauseCommand : ICommand
    {
        private Song song;
        private SongPlayingViewModel songPlayingVM;
        private bool canExecuteVariable;

        public PlayPauseCommand(Song song, SongPlayingViewModel songPlayingVM)
        {
            this.song = song;
            this.songPlayingVM = songPlayingVM;
            
            FrameworkDispatcher.Update();
            if (song != null)
            {
                canExecuteVariable = true;
                MediaPlayer.Play(song);
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteVariable;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                canExecuteVariable = true;
                MediaPlayer.Resume();
                songPlayingVM.PlaySongPictureSource = "/Assets/MusicButtons/PauseButton.png";
            }
            else if (MediaPlayer.State == MediaState.Playing)
            {
                canExecuteVariable = true;
                MediaPlayer.Pause();
                songPlayingVM.PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
            }
            else
            {
                if (song != null)
                {
                    canExecuteVariable = true;
                    MediaPlayer.Play(song);
                    songPlayingVM.PlaySongPictureSource = "/Assets/MusicButtons/PauseButton.png";
                }
                else
                {
                    canExecuteVariable = false;
                    songPlayingVM.PlaySongPictureSource = "/Assets/MusicButtons/PlayButton.png";
                }
            }
        }
    }
}
