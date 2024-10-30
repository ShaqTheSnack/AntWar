using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntEngine;

namespace ShowGame.ViewModels
{
    public class StartGameViewModel : ViewModelBase, INotifyPropertyChanged
    {
        Map Map { get; set; }
        readonly private MainWindowViewModel main;

        public StartGameViewModel(MainWindowViewModel main)
        {
            this.main = main;
            //var players = new List<Type> { typeof(TestAnt1) };
            //Map = new Map(40, 20, players, startAnts: 1, PlayMode.SingleTraining);
        }


        public void ExitBtn()
        {
            main.SetViewModel(new StartScreenViewModel(main));
        }
    }
}
