using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowGame.ViewModels
{
    public class StartScreenViewModel : ViewModelBase
    {
        readonly private MainWindowViewModel main;
        public StartScreenViewModel(MainWindowViewModel main)
        {
            this.main = main;
        }

        public void StartGamePage()
        {
            main.SetViewModel(new StartGameViewModel(main));
        }

        public void RulesPage()
        {
            main.SetViewModel(new GameRulesViewModel(main));
        }
    }
}
