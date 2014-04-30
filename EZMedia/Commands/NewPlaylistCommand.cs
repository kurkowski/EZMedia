using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EZMedia.Commands
{
    public class NewPlaylistCommand : ICommand
    {
        public NewPlaylistCommand()
        {
            
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {

        }
    }
}
