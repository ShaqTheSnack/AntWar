using AntEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WPFApp
{
    public class RonnieColemant : AntBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static int AntCount { get; set; } = 0;
        private readonly Random rnd = new Random();

        private int myCount;
        private int X { get; set; }
        private int Y { get; set; }
        private int FoodX { get; set; }
        private int FoodY { get; set; }
        private bool HasFood { get; set; } = false;
        private int CollectedFoodTwice { get; set; } = 0;
        private bool GotHomeAndReadyToDefend { get; set; } = false;
        private int DefendSteps { get; set; } = 0;
        private bool DefendSetup { get; set; } = false;

        private enum AntState
        {
            SearchingFood,
            ReturningHome,
            ReturningToFood,
            HandOutMissions,
            AttackMission,
            DefendBaseMission,
            SpyMission
        }

        private AntState currentState = AntState.SearchingFood;

        public RonnieColemant()
        {
            myCount = AntCount++;
        }

        public override void Move(ScopeData scope, List<AntBase> mates)
        {
            switch (currentState)
            {
                case AntState.SearchingFood:
                    SearchForFood(scope);
                    break;

                case AntState.ReturningHome:
                    GoHome();
                    if (AtHomeBase())
                    {
                        HasFood = false;
                        CollectedFoodTwice++;
                        currentState = CollectedFoodTwice >= 3
                            ? AntState.HandOutMissions
                            : AntState.ReturningToFood;
                    }
                    break;

                case AntState.ReturningToFood:
                    ReturnToFood();
                    if (AtFoodLocation())
                    {
                        if (scope.Center.NumFood > 0)
                        {
                            currentState = AntState.SearchingFood;
                        }
                        else
                        {
                            HasFood = false;
                            currentState = CollectedFoodTwice >= 3
                                ? AntState.HandOutMissions
                                : AntState.SearchingFood;
                        }
                    }
                    break;

                case AntState.HandOutMissions:
                    AssignMissions();
                    break;

                case AntState.AttackMission:
                    AttackAnts(scope);
                    break;

                case AntState.DefendBaseMission:
                    DefendBase(scope);
                    break;

                case AntState.SpyMission:
                    SpyOnAnts();
                    break;
            }
        }

        private void SearchForFood(ScopeData scope)
        {
            if (scope.Center.NumFood > 0)
            {
                HasFood = true;
                FoodX = X;
                FoodY = Y;
                Stay(true);
                currentState = AntState.ReturningHome;
            }
            else
            {
                MoveTowardsFood(scope);
            }
        }

        private void MoveTowardsFood(ScopeData scope)
        {
            if (scope.North.NumFood > 0) { MoveNorth(); }
            else if (scope.South.NumFood > 0) { MoveSouth(); }
            else if (scope.East.NumFood > 0) { MoveEast(); }
            else if (scope.West.NumFood > 0) { MoveWest(); }
            else { RandomMove(); }
        }

        private void RandomMove()
        {
            switch (rnd.Next(4))
            {
                case 0: MoveNorth(); break;
                case 1: MoveSouth(); break;
                case 2: MoveEast(); break;
                case 3: MoveWest(); break;
            }
        }

        private void GoHome()
        {
            MoveTowards(0, 0);
        }

        private void ReturnToFood()
        {
            MoveTowards(FoodX, FoodY);
        }

        private void AssignMissions()
        {
            int mission = rnd.Next(0, 3);
            currentState = mission switch
            {
                //0 => AntState.AttackMission,
                //1 => AntState.SpyMission,
                _ => AntState.DefendBaseMission, //This is the default case
            };
        }

        private void AttackAnts(ScopeData scope)
        {
            if (scope.North.NumAnts > 0 && scope.North.Team != Index) MoveNorth();
            else if (scope.South.NumAnts > 0 && scope.South.Team != Index) MoveSouth();
            else if (scope.East.NumAnts > 0 && scope.East.Team != Index) MoveEast();
            else if (scope.West.NumAnts > 0 && scope.West.Team != Index) MoveWest();
        }

        private void DefendBase(ScopeData scope)
        {
            if (!GotHomeAndReadyToDefend)
            {
                GoHome();
                if (AtHomeBase()) GotHomeAndReadyToDefend = true;
            }
            else
            {
                PatrolBaseTest();
            }
        }

        private void PatrolBase()
        {
            const int STEP_DURATION = 30;
            if (DefendSetup == false)
            {

            }
            else if (DefendSetup == true)
            {

                DefendSteps = (DefendSteps + 1) % (STEP_DURATION * 6);

                if (DefendSteps < STEP_DURATION) MoveNorth();
                else if (DefendSteps < STEP_DURATION * 2) MoveEast();
                else if (DefendSteps < STEP_DURATION * 3) MoveSouth();
                else if (DefendSteps < STEP_DURATION * 4) MoveWest();
            }
        }

        private void PatrolBaseTest()
        {
            if (DefendSteps == 100)
            {
                DefendSteps = 21;
            }

            if (DefendSteps < 10) MoveNorth();
            else if (DefendSteps < 20) MoveEast();
            else if (DefendSteps < 40) MoveSouth();
            else if (DefendSteps < 60) MoveWest();
            else if (DefendSteps < 80) MoveNorth();
            else if (DefendSteps < 100) MoveEast();

            DefendSteps++;
        }


        private void SpyOnAnts()
        {
            MoveWest();
        }

        private void MoveTowards(int targetX, int targetY)
        {
            if (X > targetX) MoveWest();
            else if (X < targetX) MoveEast();
            else if (Y > targetY) MoveNorth();
            else if (Y < targetY) MoveSouth();
        }

        private bool AtHomeBase() => X == 0 && Y == 0;
        private bool AtFoodLocation() => X == FoodX && Y == FoodY;

        private void MoveNorth() { North(true); Y--; }
        private void MoveSouth() { South(true); Y++; }
        private void MoveEast() { East(true); X++; }
        private void MoveWest() { West(true); X--; }
    }
}
