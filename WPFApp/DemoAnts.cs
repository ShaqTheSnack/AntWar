using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp
{
    using global::AntEngine;
    using System;
    using System.Collections.Generic;

    namespace AntEngine
    {
        public class AntDiagonal : AntBase
        {
            private bool goingNorthEast = true;
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                if (goingNorthEast)
                {
                    DX = 1;
                    DY = -1;
                }
                else
                {
                    DX = -1;
                    DY = 1;
                }
                goingNorthEast = !goingNorthEast;
            }
        }

        public class AntZigZag : AntBase
        {
            private bool moveNorth = true;
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                if (moveNorth)
                    North();
                else
                    East();

                moveNorth = !moveNorth;
            }
        }

        public class AntLoner : AntBase
        {
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                int minAnts = Math.Min(
                    Math.Min(scope.North.NumAnts, scope.South.NumAnts),
                    Math.Min(scope.East.NumAnts, scope.West.NumAnts)
                );

                if (scope.North.NumAnts == minAnts) North();
                else if (scope.South.NumAnts == minAnts) South();
                else if (scope.East.NumAnts == minAnts) East();
                else West();
            }
        }

        public class AntCircular : AntBase
        {
            private int direction = 0;
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                switch (direction)
                {
                    case 0: North(); break;
                    case 1: East(); break;
                    case 2: South(); break;
                    case 3: West(); break;
                }
                direction = (direction + 1) % 4;
            }
        }

        public class AntExplorer : AntBase
        {
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                if (scope.Center.Base) South();
                else if (scope.South.Base) North();
                else East();
            }
        }

        public class FoodCollecterAnt : AntBase
        {
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                if (scope.North.NumFood > 0) North(true);
                else if (scope.South.NumFood > 0) South(true);
                else if (scope.East.NumFood > 0) East(true);
                else if (scope.West.NumFood > 0) West(true);
                else North();
            }
        }

        public class AttackerAnt : AntBase
        {
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                if (scope.North.NumAnts > 0 && scope.North.Team != Index) North();
                else if (scope.South.NumAnts > 0 && scope.South.Team != Index) South();
                else if (scope.East.NumAnts > 0 && scope.East.Team != Index) East();
                else if (scope.West.NumAnts > 0 && scope.West.Team != Index) West();
                else North();
            }
        }

        public class AntWanderer : AntBase
        {
            private Random random = new Random();
            public override void Move(ScopeData scope, List<AntBase> mates)
            {
                int direction = random.Next(4);
                switch (direction)
                {
                    case 0: North(); break;
                    case 1: South(); break;
                    case 2: East(); break;
                    case 3: West(); break;
                }
            }
        }

    }

}
