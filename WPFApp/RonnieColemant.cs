using AntEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using static System.Formats.Asn1.AsnWriter;

namespace WPFApp
{
    public class RonnieColemant : AntBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public RonnieColemant()
        {
            myCount = AntCount++;
        }


        static int AntCount { get; set; } = 0;
        int myCount { get; set; }

        Random rnd = new Random();

        int X { get; set; }
        int Y { get; set; }
        int FoodX { get; set; }
        int FoodY { get; set; }
        bool HasFood { get; set; } = false;
        bool Missions { get; set; }
        bool FoundEnemy { get; set; }
        bool GotHomeAndReadyToDefend { get; set; } = false;




        enum AntState
        {
            //Food Collecting
            SearchingFood,
            ReturningHome,
            ReturningToFood,

            //Explore and assign
            ExploreMap,

            //Missions
            AttackMission,
            DefendBaseMission,
            AttackBaseMission,
            SpyMission


        }
        AntState currentState = AntState.SearchingFood;

        public void RandomMovement(ScopeData scope)
        {
            if (scope.Center.NumFood > 0)
            {
                HasFood = true;
                FoodX = X;
                FoodY = Y;
                Stay(true);
            }
            else if (scope.North.NumFood > 0 && !HasFood) { North(true); Y--; }
            else if (scope.South.NumFood > 0 && !HasFood) { South(true); Y++; }
            else if (scope.East.NumFood > 0 && !HasFood) { East(true); X++; }
            else if (scope.West.NumFood > 0 && !HasFood) { West(true); X--; }
            else if (!HasFood) { RandomMove(); }
        }

        private void RandomMove()
        {
            int direction = rnd.Next(0, 4);
            switch (direction)
            {
                case 0: North(true); Y--; break;
                case 1: South(true); Y++; break;
                case 2: East(true); X++; break;
                case 3: West(true); X--; break;
            }
        }

        private void MoveTowards(int targetX, int targetY)
        {
            if (X > targetX) { West(true); X--; }
            else if (X < targetX) { East(true); X++; }
            else if (Y > targetY) { North(true); Y--; }
            else if (Y < targetY) { South(true); Y++; }
        }



        public void GoHome()
        {
            if (X == 0 && Y == 0)
            {
                HasFood = false;
                FoodX = 0;
                FoodY = 0;
            }
            else
            {
                MoveTowards(0, 0);
                
            }
        }

        public void ReturnToFood()
        {
            MoveTowards(FoodX, FoodY);

        }

        private void Explore(ScopeData scope)
        {

        }

        public void AttackAnts(ScopeData scope)
        {
            if (scope.North.NumAnts > 0 && scope.North.Team != Index) North();
            else if (scope.South.NumAnts > 0 && scope.South.Team != Index) South();
            else if (scope.East.NumAnts > 0 && scope.East.Team != Index) East();
            else if (scope.West.NumAnts > 0 && scope.West.Team != Index) West();
            else North();
        }

        public void DefendBase(ScopeData scope)
        {
            South(true);
        }

        public void SpyOnAnts(ScopeData scope)
        {
            West(true);
        }

        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            switch (currentState)
            {
                case AntState.SearchingFood:
                    RandomMovement(scope);
                    if (HasFood)
                    {
                        currentState = AntState.ReturningHome;
                    }
                    break;

                case AntState.ReturningHome:
                    GoHome();
                    if (X == 0 && Y == 0)
                    {
                        currentState = AntState.ReturningToFood;
                    }
                    break;

                case AntState.ReturningToFood:
                    ReturnToFood();

                    if (X == FoodX && Y == FoodY)
                    {
                        if (scope.Center.NumFood > 0)
                        {
                            currentState = AntState.SearchingFood;
                        }
                        if (scope.Center.NumFood == 0 && X == FoodX && Y == FoodY)
                        {
                            HasFood = false;
                        }
                        if (!HasFood)
                        {
                            currentState = AntState.ExploreMap;
                        }

                    }

                    break;

                case AntState.ExploreMap:
                    Explore(scope);
                    if (Missions == true)
                    {
                        int assignMissions = rnd.Next(0, 3);
                        switch (assignMissions)
                        {
                            case 0: currentState = AntState.AttackMission; break;
                            case 1: currentState = AntState.DefendBaseMission; break;
                            case 2: currentState = AntState.SpyMission; break;
                        }
                    }
                    break;

                case AntState.AttackMission:
                    AttackAnts(scope);
                    break;

                case AntState.DefendBaseMission:
                    if (GotHomeAndReadyToDefend == false)
                    {
                        GoHome();
                    }
                    if (X == 0 && Y == 0)
                    {
                        GotHomeAndReadyToDefend = true;
                        DefendBase(scope);
                    }

                    break;

                case AntState.SpyMission:

                    SpyOnAnts(scope);
                    break;
            }
        }

        //public override void Move(ScopeData scope, List<AntBase> mates)
        //{
        //    if (scope.Center.NumFood > 0 && mates.Count > 3)
        //    {
        //        // Hvis der er mad til stede og nok allierede er i nærheden, så bliv og forsvar maden
        //        Stay();
        //    }
        //    else
        //    {
        //        // Ellers gør noget andet
        //        North();
        //    }
        //}
    }
}
