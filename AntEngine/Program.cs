using System.Diagnostics;

namespace AntEngine
{
    class Program
    {
        //Min næste opgave er at lave logikken for en myre der retunere mad
        static void Main(string[] args)
        {

            //var players = new List<Type> { typeof(TestAntNorth), typeof(TestAntSouth) };
            //var players = new List<Type> { typeof(TestAntNorth)};
            var players = new List<Type> { typeof(RandomAnt) };

            var map = new Map(10, 10, players, startAnts: 1, PlayMode.SingleTraining);
            for (int count = 0; count < 40; count++)
            {
                map.DisplayMap();
                Console.ReadLine();
                map.PlayRound();
            }
        }
    }

    public class RandomAnt : AntBase
    {
        int steps { get; set; }
        SquareData squareData = new();
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            Random rnd = new Random();
            steps = rnd.Next(1, 5);
            if (steps == 1)
            {
                North(true);
            }
            if (steps == 2)
            {
                South(true);
            }
            if (steps == 3)
            {
                West(true);
            }
            if (steps == 4)
            {
                East(true);
            }

        }
    }

    public class GoBackAnt : AntBase
    {
        SquareData squareData = new();
        int has_no_food = 0;
        int steps = 0;
        int step = 0;
        public override void Move(ScopeData scope, List<AntBase> mates)
        {


            if (has_no_food == scope.Center.NumFood)
            {
                North(true);
                steps++;
                
            }
            else if (has_no_food != scope.Center.NumFood) 
            {
                South(true);
                steps--;

            }
        }
    }
}
