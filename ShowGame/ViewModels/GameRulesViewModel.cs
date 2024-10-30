using Avalonia.Controls.Primitives;
using ReactiveUI;
using ShowGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowGame.ViewModels
{
    public class GameRulesViewModel : ViewModelBase
    {
        readonly RulesModel rm = new();
        readonly private MainWindowViewModel main;
        public GameRulesViewModel(MainWindowViewModel main)
        {
            this.main = main;
            Rules = rm.Rules;
        }


        private string rules;
        public string Rules
        {
            get => rules;
            set => this.RaiseAndSetIfChanged(ref rules, value);
        }


        public void BackBtn()
        {
            main.SetViewModel(new StartScreenViewModel(main));
        }
    }
}
