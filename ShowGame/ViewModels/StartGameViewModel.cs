using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntEngine;
using Avalonia.Media;
using ReactiveUI;

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
            StartGame();
        }

        private string color;
        public string Color
        {
            get => color;
            set => this.RaiseAndSetIfChanged(ref color, value);
        }

        public void StartGame()
        {
            for (int i = 0; i < 10; i++)
            {
                Color = "Red";
            }

        }


        public void ExitBtn()
        {
            main.SetViewModel(new StartScreenViewModel(main));
        }
    }
}
