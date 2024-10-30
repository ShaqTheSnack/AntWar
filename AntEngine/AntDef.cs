using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntEngine
{
    public class AntDef
    {
        // Maximum number of ants in one square
        public int MaxSquareAnts { get; set; } = 100;

        // Maximum number of food in one square
        public int MaxSquareFood { get; set; } = 200;

        // Number of ant required to build a new base
        public int NewBaseAnts { get; set; } = 25;

        // Number of food required to build a new base
        public int NewBaseFood { get; set; } = 50;

        // Point for one base
        public int BaseValue { get; set; } = 50;


        public int TimeOutTurn { get; set; } = 20000;
        public int WinPercent { get; set; } = 75;
        public int NumBattles { get; set; } = 100;

        public int MinMapSize { get; set; } = 250;
        public int MaxMapSize { get; set; } = 500;

        public bool RaiseException { get; set; } = true;
    }
}
