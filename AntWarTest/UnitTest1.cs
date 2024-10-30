
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

        [Fact]
        public void TestSafeX()
        {
            List<Type> ants = [typeof(TestAntSouth)];

            var my_map = new Map(5, 5, ants, 1, PlayMode.SingleTraining);

            Assert.Equal(3, my_map.SafeX(3));
            Assert.Equal(4, my_map.SafeX(4));
            Assert.Equal(0, my_map.SafeX(5));

            Assert.Equal(1, my_map.SafeX(1));
            Assert.Equal(0, my_map.SafeX(0));
            Assert.Equal(4, my_map.SafeX(-1));

        }

        [Fact]
        public void TestSafeY()
        {
            List<Type> ants = [typeof(TestAntSouth)];

            var my_map = new Map(5, 5, ants, 1, PlayMode.SingleTraining);

            Assert.Equal(3, my_map.SafeY(3));
            Assert.Equal(4, my_map.SafeY(4));
            Assert.Equal(0, my_map.SafeY(5));

            Assert.Equal(1, my_map.SafeY(1));
            Assert.Equal(0, my_map.SafeY(0));
            Assert.Equal(4, my_map.SafeY(-1));

        }
    }
}