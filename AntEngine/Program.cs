namespace AntEngine
{
    class Program
    {
        static void Main(string[] args)
        {

            // var players = new List<Type> { typeof(TestAntNorth), typeof(TestAntSouth) };
            var players = new List<Type> { typeof(TestAntNorth)};

            var map = new Map(20, 20, players, startAnts: 1, PlayMode.SingleTraining);
            for (int count = 0; count < 40; count++)
            {
                map.DisplayMap();
                Console.ReadLine();
                map.PlayRound();
            }
        }
    }
}
