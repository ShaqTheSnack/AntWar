using AntEngine;

namespace AntWarTest
{
    public class SingleTrainingLoopTest
    {
        [Fact]
        public void TestMethod1()
        {
            int width = 20;
            int height = 20;
            int number_of_ants = 5;
            List<Type> ants = [typeof(TestAntNorth)];
            List<Position> PositionXY = new List<Position>();
            PositionXY.AddRange([new Position { x = 1, y = 1 }, new Position { x = 5, y = 6 }]);

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining, PositionXY);
            Assert.NotNull(my_map);

            // Place food at positions for PositionXY
            foreach (var pos in PositionXY)
            {
                my_map.PlaceFoodTest(1, pos.x, pos.y);
            }

            // Check food placement is correct
            foreach (var pos in PositionXY)
            {
                Assert.True(my_map.FoodMap[pos.x, pos.y] > 0, $"Food was not placed at ({pos.x}, {pos.y}).");
            }

            for (int count = 0; count < 5; count++)
            {
                var field = my_map.GetAnts(10, 10 - count);
                Assert.NotNull(field);
                Assert.Equal(field.Count, number_of_ants);

                my_map.PlayRound();
            }
        }

        [Fact]
        public void TestLoopMapSouth()
        {
            int width = 10;
            int height = 10;
            int number_of_ants = 3;
            List<Type> ants = [typeof(TestAntSouth)];
            List<AntBase>? field;

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining);
            Assert.NotNull(my_map);

            // assert all ants in center pos    
            field = my_map.GetAnts(width / 2, height / 2);
            Assert.NotNull(field);
            Assert.Equal(number_of_ants, field.Count);

            for (int count = 0; count < height-1; count++)
            {
                my_map.PlayRound();

                // assert no ants in center pos    
                field = my_map.GetAnts(width / 2, height / 2);
                Assert.NotNull(field);
                Assert.Empty(field);
            }
            my_map.PlayRound();

            // assert all ants in center pos    
            field = my_map.GetAnts(width / 2, height / 2);
            Assert.NotNull(field);
            Assert.Equal(number_of_ants, field.Count);
        }

        [Fact]
        public void TestLoopMapNorth()
        {
            int width = 10;
            int height = 10;
            int number_of_ants = 3;
            List<Type> ants = [typeof(TestAntNorth)];
            List<AntBase>? field;

            var my_map = new Map(width, height, ants, number_of_ants, PlayMode.SingleTraining);
            Assert.NotNull(my_map);

            // assert all ants in center pos    
            field = my_map.GetAnts(width / 2, height / 2);
            Assert.NotNull(field);
            Assert.Equal(number_of_ants, field.Count);

            for (int count = 0; count < 9; count++)
            {
                my_map.PlayRound();

                // assert no ants in center pos    
                field = my_map.GetAnts(width / 2, height / 2);
                Assert.NotNull(field);
                Assert.Empty(field);
            }
            my_map.PlayRound();

            // assert all ants in center pos    
            field = my_map.GetAnts(width / 2, height / 2);
            Assert.NotNull(field);
            Assert.Equal(number_of_ants, field.Count);
        }
    }
}