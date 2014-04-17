using EZMedia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EZMedia.Commands
{
    public class ShuffleSongsCommand : ICommand
    {
        private EZMediaPlayer _mediaPlayer;
        private SongPlayingViewModel _viewModel;

        public ShuffleSongsCommand(EZMediaPlayer mediaPlayer, SongPlayingViewModel viewModel)
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
                if (_mediaPlayer.IsShuffled)
                {
                    _mediaPlayer.IsShuffled = false;
                    _viewModel.ShuffleSongPictureSource = "/Assets/MusicButtons/ShuffleSongsNotClicked.png";
                }
                else
                {
                    _mediaPlayer.IsShuffled = true;
                    _viewModel.ShuffleSongPictureSource = "/Assets/MusicButtons/ShuffleSongs.png";
                }
            }
        }
    }
}
