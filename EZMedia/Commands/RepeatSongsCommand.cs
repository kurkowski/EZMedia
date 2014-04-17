using EZMedia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EZMedia.Commands
{
    public class RepeatSongsCommand : ICommand
    {
        private EZMediaPlayer _mediaPlayer;
        private SongPlayingViewModel _viewModel;

        public RepeatSongsCommand(EZMediaPlayer mediaPlayer, SongPlayingViewModel viewModel)
        {
            _mediaPlayer = mediaPlayer;
            _viewModel = viewModel;
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
                if (_mediaPlayer.IsRepeating)
                {
                    _mediaPlayer.IsRepeating = false;
                    _viewModel.RepeatSongPictureSource = "/Assets/MusicButtons/RepeatSongsNotClicked.png";
                }
                else
                {
                    _mediaPlayer.IsRepeating = true;
                    _viewModel.RepeatSongPictureSource = "/Assets/MusicButtons/RepeatSongs.png";
                }
            }
        }
    }
}
