
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Transactions;

namespace AntEngine
{
    public class SquareData
    {
        public int NumAnts { get; set; } = 0;
        public bool Base { get; set; } = false;
        public int Team { get; set; } = 0;
        public int NumFood { get; set; } = 0;

        public override string ToString()
        {
            return $"Team: {Team}, " +
                   $"NumAnts: {NumAnts}, " +
                   $"NumFood: {NumFood}, " +
                   $"Base: {Base}";
        }
    }

    public class ScopeData
    {
        public SquareData Center { get; set; }
        public SquareData North { get; set; }
        public SquareData South { get; set; }
        public SquareData East { get; set; }
        public SquareData West { get; set; }

        public ScopeData()
        {
            Center = new SquareData();
            North = new SquareData();
            South = new SquareData();
            East = new SquareData();
            West = new SquareData();
        }

        public override string ToString()
        {
            return $"center: {Center}";
        }
    }

    public enum PlayMode
    {
        SingleTraining, // single ant species always placed in the middle of the map. For debugging / unit testing 
        DuoMatch, // Two ant species placed symmetrically on the map
        Game, // N species placed randomly on the map
    }
    public class Position
    {
        public int x;
        public int y;
    }

    public class Map
    {
        int Width { get; init; }
        int Height { get; init; }
        public int RoundNo { get; private set; }

        readonly List<AntBase>[,] GridMap;
        readonly List<Type> Players;
        readonly List<AntHome> AntHomes;
        public readonly int[,] FoodMap;
        
        

        private Dictionary<int, int> statistics = new Dictionary<int, int>();

        public Map(int width, int height, List<Type> players, int startAnts = 1, PlayMode mode = PlayMode.SingleTraining, List<Position>? predefinedPositions = null)
        {
            RoundNo = 0;
            Width = width;
            Height = height;
            Players = players;
            AntHomes = new List<AntHome>();
            FoodMap = new int[Width, Height];
            GridMap = new List<AntBase>[Width, Height];

            int Index = 0;
            Random rnd = new Random();

            // validate number of ants species
            if (Players.Count == 0)
                throw new Exception("Wrong number of ant species!");

            if ((mode == PlayMode.SingleTraining) && (Players.Count != 1))
                throw new Exception("Wrong number of ant species!");

            else if ((mode == PlayMode.DuoMatch) && (Players.Count != 2))
                throw new Exception("Wrong number of ant species!");


            foreach (Type species in Players)
            {
                AntHome home;
                switch (mode)
                {
                    case PlayMode.SingleTraining:
                        home = new AntHome { X = Width / 2, Y = Height / 2, Species = species };
                        break;
                    case PlayMode.DuoMatch:
                        // TO DO Fix placement
                        home = new AntHome { X = Width / 3 * (Index + 1), Y = Height / 3 * (Index + 1), Species = species };
                        break;

                    case PlayMode.Game:
                        var randomWidth = rnd.Next(0, width);
                        var randomHeight = rnd.Next(0, height);
                        home = new AntHome { X = randomWidth, Y = randomHeight, Species = species };
                        break;
                    default:
                        throw new Exception("Unknown mode: " + mode);
                }
                AntHomes.Add(home);
                for (int count = 0; count < startAnts; count++)
                {
                    Object? o = Activator.CreateInstance(species);
                    if (o != null)
                    {
                        AntBase ant = (AntBase)o;
                        ant.Name = species.Name;
                        ant.Index = Index;
                        Add(ant, home.X, home.Y);
                    }
                }
                Index++;
            }
        }

        public void PlaceFood(int numberOfFoods)
        {
            Random rnd = new Random();

            for (int i = 0; i < numberOfFoods; i++)
            {
                int foodX, foodY;

                foodX = rnd.Next(0, Width);
                foodY = rnd.Next(0, Height);
                FoodMap[foodX, foodY] = rnd.Next(5, 10);
            }
        }

        public void PlaceFoodTest(int numberOfFoods, int foodPosX, int foodPosY)
        {
            Random rnd = new Random();

            for (int i = 0; i < numberOfFoods; i++)
            {
                int foodX, foodY;

                foodX = foodPosX;
                foodY = foodPosY;
                FoodMap[foodX, foodY] = rnd.Next(5, 10);
            }
        }



        public List<AntBase>? GetAnts(int x, int y)
        {
            var tmp = GridMap[x, y];
            if (tmp == null)
                return tmp;
            else
                return new List<AntBase>(tmp);
        }

        public void Add(AntBase a, int x, int y)
        {
            // move to an empty cell
            if (GridMap[x, y] == null)
            {
                GridMap[x, y] = [];
                GridMap[x, y].Add(a);
            }
            // move to an empty list
            else if (GridMap[x, y].Count == 0)
            {
                GridMap[x, y].Add(a);
            }
            else
            {
                var first = GridMap[x, y][0];

                // move to a cell with friends
                if (first.Name == a.Name)
                {
                    GridMap[x, y].Add(a);
                } // hit
                else
                {
                    GridMap[x, y] = [a];
                }
            }
        }

        public Dictionary<int, int> GetStatistics()
        {
            return statistics;
        }

        public void PlayRound()
        {

            RoundNo++;
            PlaceFood(0);
            List<AntItem> ToProcess = [];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var field = GridMap[x, y];
                    if (field != null)
                    {
                        foreach (AntBase a in field)
                        {
                            ToProcess.Add(new AntItem { X = x, Y = y, Ant = a });
                        }
                    }
                }
            }
            // shuffle list
            ToProcess = ToProcess.OrderBy(x => Guid.NewGuid()).ToList();
            statistics = new();
            foreach (AntItem item in ToProcess)
            {
                if(statistics.ContainsKey(item.Ant.Index))
                    statistics[item.Ant.Index] += 1;
                else
                    statistics[item.Ant.Index] = 0;
                ScopeData sc = CheckForScope(item.X, item.Y);

                List<AntBase> mates = GridMap[item.X, item.Y].Where(a => a != item.Ant).ToList();
                item.Ant.Move(sc, mates);

                int new_x = SafeX(item.X + item.Ant.DX);
                int new_y = SafeY(item.Y + item.Ant.DY);


                if (FoodMap[item.X, item.Y] > 0)
                {
                    if (item.Ant.WithFood)
                    {
                        FoodMap[item.X, item.Y]--;
                        AntHome myHome = AntHomes[item.Ant.Index];
                        if(myHome.X == new_x && myHome.Y == new_y)
                        {
                            Object? o = Activator.CreateInstance(myHome.Species);
                            if (o != null)
                            {
                                AntBase ant = (AntBase)o;
                                ant.Name = myHome.Species.Name;
                                ant.Index = item.Ant.Index;
                                Add(ant, new_x, new_y);
                            }
                        }
                        else
                        {
                            FoodMap[new_x, new_y]++;
                        }

                    }
                }

                List<AntBase> from = GridMap[item.X, item.Y];
                from.Remove(item.Ant);
                Add(item.Ant, new_x, new_y);


            }
        }
        public void DisplayMap()
        {
            Console.WriteLine($"Round: {RoundNo}");
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (GridMap[x, y] != null && GridMap[x, y].Count > 0)
                    {
                        Console.Write("A");
                    }
                    else if (FoodMap[x, y] > 0)
                    {
                        Console.Write("F");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        public int SafeX(int x)
        {
            if (x < 0)
                return Width - 1;
            else if (x == Width)
                return 0;
            else
                return x;
        }
        public int SafeY(int y)
        {
            if (y < 0)
                return Height - 1;
            else if (y == Height)
                return 0;
            else
                return y;
        }

        public int AntNumber(int x, int y)
        {
            var field = GridMap[x, y];
            if (field == null)
            {
                return 0;
            }
            else
            {
                return field.Count;
            }
        }

        public int AntTeamNumber(int x, int y)
        {
            var field = GridMap[x, y];
            if (field == null)
            {
                return 0;
            }
            else
            {
                if (field.Count == 0)
                    return 0;
                else
                    return field[0].Index;
            }
        }

        public bool AntHome(int x, int y)
        {
            foreach (var ant in AntHomes)
            {

                if (ant.X == x && ant.Y == y)
                {
                    return true;
                }

            }
            return false;
        }



        public ScopeData CheckForScope(int x, int y)
        {
            ScopeData sc = new();

            sc.Center.NumAnts = AntNumber(x, y);
            sc.North.NumAnts = AntNumber(x, SafeY(y - 1));
            sc.South.NumAnts = AntNumber(x, SafeY(y + 1));
            sc.East.NumAnts = AntNumber(SafeX(x + 1), y);
            sc.West.NumAnts = AntNumber(SafeX(x - 1), y);

            sc.Center.NumFood = FoodMap[x, y];
            sc.North.NumFood = FoodMap[x, SafeY(y - 1)];
            sc.South.NumFood = FoodMap[x, SafeY(y + 1)];
            sc.East.NumFood = FoodMap[SafeX(x + 1), y];
            sc.West.NumFood = FoodMap[SafeX(x - 1), y];

            sc.Center.Team = AntTeamNumber(x, y);
            sc.North.Team = AntTeamNumber(x, SafeY(y - 1));
            sc.South.Team = AntTeamNumber(x, SafeY(y + 1));
            sc.East.Team = AntTeamNumber(SafeX(x + 1), y);
            sc.West.Team = AntTeamNumber(SafeX(x - 1), y);

            sc.Center.Base = AntHome(x, y);
            sc.North.Base = AntHome(x, SafeY(y - 1));
            sc.South.Base = AntHome(x, SafeY(y + 1));
            sc.East.Base = AntHome(SafeX(x + 1), y);
            sc.West.Base = AntHome(SafeX(x - 1), y);
            return sc;
        }

    }


    public record AntItem
    {
        public int X { get; init; }
        public int Y { get; init; }
        public required AntBase Ant { get; init; }
    }


    // simple test ant implementation for used in UnitTest etc.
    public class TestAntNorth : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            North();
        }
    }

    public class TestAntSouth : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            South();
        }
    }
    public class TestAntWest : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            West();
        }
    }

    public class TestAntEast : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            East();
        }
    }

    public class AntHome
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Type Species { get; set; }
    }

    public class Food
    {
        public int FoodPerRound { get; set; }

        public Food(int foodPerRound)
        {
            FoodPerRound = foodPerRound;
        }
    }

}