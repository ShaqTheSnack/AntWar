using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntwarConsoleProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            var players = new List<Type> { typeof(TestAnt1), typeof(TestAnt2) };
            var map = new Map(40, 20, players, startAnts: 1);
            map.DisplayMap();
        }


    }
}

