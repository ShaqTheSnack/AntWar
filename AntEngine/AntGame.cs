using AntwarConsoleProgram;
using System;

namespace AntwarConsoleProgram
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

    public class Map
    {
        int Width { get; set; }
        int Height { get; set; }

        readonly List<AntBase>[,] GridMap;
        readonly List<Type> Players;
        readonly List<AntHome> AntHomes;
        readonly int[,] FoodMap;

        // TODO: Shaq, two dim array of ints with food info

        public Map(int width, int height, List<Type> players, int startAnts = 1)
        {
            Width = width;
            Height = height;
            Players = players;
            AntHomes = new List<AntHome>();
            FoodMap = new int[Width, Height];

            GridMap = new List<AntBase>[Width, Height];

            int Index = 0;
            Random rnd = new Random();

            foreach (Type species in Players)
            {
                var randomWidth = rnd.Next(0, width);
                var randomHeight = rnd.Next(0, height);
                var home = new AntHome { X = randomWidth, Y = randomHeight };
                AntHomes.Add(home);

                for (int count = 0; count < startAnts; count++)
                {
                    Object? o = Activator.CreateInstance(species);
                    if (o != null)
                    {
                        AntBase ant = (AntBase)o;
                        ant.Name = species.Name;
                        ant.Attach(this, Index);
                        Add(ant, home.X, home.Y);
                    }
                }
                Index++;
            }
        }

        private void PlaceFood(int numberOfFoods)
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
            if (GridMap[x, y] == null)
                GridMap[x, y] = [];

            GridMap[x, y].Add(a);
        }

        public void Move_Delta(AntBase ant, int dx, int dy, bool with_food = false)
        {
            // find ant in original list of move to new list


        }

        public void PlayRound()
        {
            int foodAmountPerRound = 2;
            PlaceFood(foodAmountPerRound);
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


            foreach (AntItem item in ToProcess)
            {
                ScopeData sc = CheckForScope(item.X, item.Y);

                List<AntBase> mates = [];
                item.Ant.Move(sc, mates);

                List<AntBase> from = GridMap[item.X, item.Y];
                List<AntBase> to = GridMap[item.X + item.Ant.DX, item.Y + item.Ant.DY];

                from.Remove(item.Ant);
                GridMap[item.X + item.Ant.DX, item.Y + item.Ant.DY] = [item.Ant];
            }
        }
        public void DisplayMap()
        {
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
            var Scopes = new[] { sc.North, sc.South, sc.Center, sc.West, sc.East };
            //foreach (var Scope in Scopes)
            //{
            //    if (Scope.NumAnts <= 1 || Scope.NumFood <= 1 || Scope.Team <= 1 || Scope.Base == true)
            //    {
            //        Console.WriteLine($"NumAnts: {Scope.NumAnts}, NumFood: {Scope.NumFood}, Team: {Scope.Team}, Base: {Scope.Base}");
            //    }
            //}



            //Center
            sc.Center.NumAnts = AntNumber(x, y);
            sc.North.NumAnts = AntNumber(x, y - 1);
            sc.South.NumAnts = AntNumber(x, y + 1);
            sc.East.NumAnts = AntNumber(x + 1, y);
            sc.West.NumAnts = AntNumber(x - 1, y);

            sc.Center.NumFood = FoodMap[x, y];
            sc.North.NumFood = FoodMap[x, y - 1];
            sc.South.NumFood = FoodMap[x, y + 1];
            sc.East.NumFood = FoodMap[x + 1, y];
            sc.West.NumFood = FoodMap[x - 1, y];

            sc.Center.Team = AntTeamNumber(x, y);
            sc.North.Team = AntTeamNumber(x, y - 1);
            sc.South.Team = AntTeamNumber(x, y + 1);
            sc.East.Team = AntTeamNumber(x + 1, y);
            sc.West.Team = AntTeamNumber(x - 1, y);

            sc.Center.Base = AntHome(x, y);
            sc.North.Base = AntHome(x, y - 1);
            sc.South.Base = AntHome(x, y + 1);
            sc.East.Base = AntHome(x + 1, y);
            sc.West.Base = AntHome(x - 1, y);
            return sc;
        }

    }


    public record AntItem
    {
        public int X { get; init; }
        public int Y { get; init; }
        public required AntBase Ant { get; init; }
    }

    public class TestAnt1 : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            throw new NotImplementedException();
        }
    }

    public class TestAnt2 : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            throw new NotImplementedException();
        }
    }

    public class AntHome
    {
        public int X { get; set; }
        public int Y { get; set; }
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