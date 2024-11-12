using AntEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace WPFApp
{
    public class PMANDT : AntBase
    {
        private int my_x = 0;
        private int my_y = 0;
        private int state = 0;

        private int Food_x = 0;
        private int Food_y = 0;



        // states:

        // 0: new - I know nothing
        // 1: I have knowledge of food
        // 2: I am carring home food
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PMANDT()
        {

        }

        public override void West(bool with_food = false)
        {
            logger.Info($"{Index}, {Name}, {my_x}, {my_y}");
            my_x -= 1;
            base.West(with_food);

        }

        public override void North(bool with_food = false)
        {
            my_y += 1;
            base.North(with_food);
        }

        public override void South(bool with_food = false)
        {
            my_x -= 1;
            base.South(with_food);
        }
        public override void East(bool with_food = false)
        {
            my_x += 1;
            base.East(with_food);
        }

        public void MoveTo(int x, int y)
        {

        }

        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            if (KillIfPossible(scope)) return;

            // state changes
            if (scope.Center.NumFood > 0)
            {
                state = 2;
            }

            // state actions

            if (state == 0)
            {

                Random rnd = new ();
                switch (rnd.Next(4))
                {
                    case 0:
                        North();
                        break;
                    case 1:
                        East();
                        break;
                    case 2:
                        West();
                        break;
                    default:
                        South();
                        break;
                }
            }
            else if (state == 1)
            {
                MoveTo(Food_x, Food_y);
            }
            else if (state == 2)
            {
                MoveTo(0, 0);
            }

            if (BringHomeFood(scope)) return;

            // bring food home

            // look for food
        }

        private bool Target(SquareData sc)
        {
            var result = (sc.NumAnts > 0) && (sc.Team != Index);
            if (result)
                logger.Info($"Possible kill: {sc.NumAnts} {sc.Team}");
            return result;
        }

        private bool KillIfPossible(ScopeData scope)
        {
            if (Target(scope.North))
            {
                North();
            }
            else if (Target(scope.South))
            {
                South();
            }
            else if (Target(scope.West))
            {
                West();
            }
            else if (Target(scope.East))
            {
                East();
            }
            else
                return false;

            return true;
        }

        private bool BringHomeFood(ScopeData scope)
        {

            return false;

        }

    }
}
