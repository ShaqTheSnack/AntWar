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
    }
}