
using AntEngine;

namespace AntWarTest
{
    public class UnitTestAntNorth : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            Console.WriteLine(scope);
            North();
        }
    }
    public class UnitTestAntSouth : AntBase
    {
        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            Console.WriteLine(scope);
            South();
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            int width = 10;
            int height = 10;
            int number_of_ants = 2;
            List<Type> ants = [typeof(UnitTestAntNorth), typeof(UnitTestAntSouth)];

            var my_map = new Map(width, height, ants, number_of_ants);

            bool found_south = false, found_north = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var field = my_map.GetAnts(x, y);
                    if (field != null)
                    {
                        if (field.Any(x => x.Name == "UnitTestAntNorth"))
                        {
                            found_north = true;
                        }
                        if (field.Any(x => x.Name == "UnitTestAntSouth"))
                        {
                            found_south = true;
                        }
                    }
                }
            }
            Assert.True(found_north);
            Assert.True(found_south);
        }
    }

    public class UnitTest2
    {
        [Fact]
        public void SquareData()
        {
            var square = new SquareData();
            Assert.Equal(0, square.NumAnts);
            Assert.Equal("Team: 0, NumAnts: 0, NumFood: 0, Base: False", square.ToString());
            Assert.Equal(0, square.Team);
            Assert.Equal(0, square.NumFood);
        }

        [Fact]
        public void SquareDataTest()
        {
            var square = new SquareData
            {
                NumAnts = 5,
                Base = true,
                Team = 1,
                NumFood = 10
            };
            string expected = "Team: 1, NumAnts: 5, NumFood: 10, Base: True";
            Assert.Equal(expected, square.ToString());
        }


        [Fact]
        public void TestMethodPlayRound()
        {
            int width = 10;
            int height = 10;
            int number_of_ants = 2;
            List<Type> ants = [typeof(UnitTestAntNorth), typeof(UnitTestAntSouth)];

            var my_map = new Map(width, height, ants, number_of_ants);

            my_map.PlayRound();
        }
    }
}