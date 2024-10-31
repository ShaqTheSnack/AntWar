namespace AntEngine
{
    class Program
    {

        static void Main(string[] args)
        {

            //var players = new List<Type> { typeof(TestAntNorth), typeof(TestAntSouth) };
            //var players = new List<Type> { typeof(TestAntNorth)};
            var players = new List<Type> { typeof(ShaquilleAnt) };

            var map = new Map(10, 10, players, startAnts: 1, PlayMode.SingleTraining);
            for (int count = 0; count < 40; count++)
            {
                map.DisplayMap();
                Console.ReadLine();
                map.PlayRound();
            }
        }
    }

    public class ShaquilleAnt : AntBase
    {
        int steps { get; set; } 
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            Random rnd = new Random();
            steps = rnd.Next(1, 5);
            if (steps == 1)
            {
                North();
            }
            if (steps == 2)
            {
                South();
            }
            if (steps == 3)
            {
                West();
            }
            if (steps == 4)
            {
                East();
            }

        }
    }
}
