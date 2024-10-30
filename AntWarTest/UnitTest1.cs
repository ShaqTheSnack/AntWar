
using AntEngine;

namespace AntWarTest
{
    public class UnitTest1
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
            List<Type> ants = [typeof(TestAntNorth), typeof(TestAntSouth)];

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining);

            my_map.PlayRound();
        }
    }
}