using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowGame.ViewModels
{
    internal class StartGameViewModel : ViewModelBase
    {
        readonly private MainWindowViewModel main;
        public StartGameViewModel(MainWindowViewModel main)
        {
            this.main = main;
        }

    }
}
