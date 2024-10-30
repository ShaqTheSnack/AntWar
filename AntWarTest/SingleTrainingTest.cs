using AntEngine;

namespace AntWarTest
{
    public class SingleTrainingTest
    {
        [Fact]
        public void TestMethod1()
        {
            int width = 20;
            int height = 20;
            int number_of_ants = 5;
            List<Type> ants = [typeof(TestAntNorth)];

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining);
            Assert.NotNull(my_map);

            for (int count = 0; count < 5; count++)
            {
                var field = my_map.GetAnts(10, 10 - count);
                Assert.NotNull(field);
                Assert.Equal(field.Count, number_of_ants);

                my_map.PlayRound();
            }
        }


        [Fact]
        public void TestLoopMapNorth()
        {
            int width = 10;
            int height = 10;
            int number_of_ants = 3;
            List<Type> ants = [typeof(TestAntSouth)];

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining);
            Assert.NotNull(my_map);

            for (int count = 0; count < 10; count++)
            {
                var field = my_map.GetAnts(width / 2 , (height / 2  + count) % width);
                Assert.NotNull(field);
                Assert.Equal(field.Count, number_of_ants);

                my_map.PlayRound();
            }
        }

    }
}