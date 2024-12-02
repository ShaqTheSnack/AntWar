using System.Diagnostics;
using AntEngine;

namespace WPFApp;

public class AntAnt : AntBase
{
    private (int x, int y) currentPOS = (0, 0);
    private List<(int x, int y, int foodCount, int distanceFromBase)> foodList = new();
    private bool initial = true;
    private int random;
    private AntState state = AntState.RandomMovement;
    private (int x, int y) targetPOS = (0, 0);
    private int MoveInTurnCounter = 0;
    private int AmoutFoodCollected = 0;
    
    private enum AntState
    {
        RandomMovement,
        GoToFood,
        GoToBase,
        explore,
    }

    public override void Move(ScopeData scope, List<AntBase> mates)
    {
        MoveInTurnCounter = 0;
        (int x, int y) currentPOScupy = currentPOS;
        if (initial)
        {
            Random rnd = new();
            random = rnd.Next(4);
            initial = false;
        }

        LookForFood(scope);
        if (currentPOS == (0, 0))
            Debug.WriteLine("ant at base");

        switch (state)
        {
            case AntState.RandomMovement:
                RandomMovement();
                break;
            case AntState.GoToFood:
                GoToFood(scope);
                break;
            case AntState.GoToBase:
                GoToBase();
                break;
            case AntState.explore:
                Explore();
                break;
        }

        if (MoveInTurnCounter == 0)
        {
            currentPOS = currentPOScupy;
            Stay();
        }
    }

    private void RandomMovement()
    {
        switch (random)
        {
            case 0:
                North();
                break;
            case 1:
                South();
                break;
            case 2:
                East();
                break;
            default:
                West();
                break;
        }
    }

    private void GoToFood(ScopeData scope)
    {
        if (!GoToTargetPOS())
            return;

        if (scope.Center.NumFood >= 1)
            foreach (var food in foodList)
                if (food.x == currentPOS.x && food.y == currentPOS.y)
                {
                    foodList.Remove(food);
                    break;
                }

        targetPOS = (0, 0);
        state = AntState.GoToBase;
        //GoToBase();
    }

    private void GoToBase()
    {
        if (!GoToTargetPOS(true))
            return;

        AmoutFoodCollected++;
        if (foodList.Count == 0)
        {
            state = AntState.RandomMovement;
            //RandomMovement();
            return;
        }

        if (AmoutFoodCollected >= 5)
        {
            state = AntState.explore;
            return;
        }

        var closestFood = foodList.OrderBy(o => o.distanceFromBase).First();
        targetPOS = (closestFood.x, closestFood.y);
        state = AntState.GoToFood;
    }

    private void Explore()
    {
        if (GoToTargetPOS())
        {
            targetPOS.x++;
            targetPOS.y++;
        }

        GoToTargetPOS();
    }

    private bool GoToTargetPOS(bool withFood = false)
    {
        if (currentPOS.y < targetPOS.y)
            North(withFood);
        else if (currentPOS.y > targetPOS.y)
            South(withFood);
        else if (currentPOS.x < targetPOS.x)
            East(withFood);
        else if (currentPOS.x > targetPOS.x)
            West(withFood);
        else
            return true;

        return false;
    }

    private void LookForFood(ScopeData scope)
    {
        if (scope.North.NumFood > 4 && scope.North.NumAnts == 0)
            FoundFood(scope.North, (0, 1));

        if (scope.South.NumFood > 4 && scope.South.NumAnts == 0)
            FoundFood(scope.South, (0, -1));

        if (scope.East.NumFood > 4 && scope.East.NumAnts == 0)
            FoundFood(scope.East, (1, 0));

        if (scope.West.NumFood > 4 && scope.West.NumAnts == 0)
            FoundFood(scope.West, (-1, 0));
    }

    private void FoundFood(SquareData square, (int x, int y) posOffset)
    {
        (int x, int y) foodPOS = (currentPOS.x + posOffset.x, currentPOS.y + posOffset.y);

        foreach (var food in foodList)
            if (food.x == foodPOS.x && food.y == foodPOS.y)
                return;

        var distanceFromBase = Math.Abs(foodPOS.x) + Math.Abs(foodPOS.y);
        foodList.Add((foodPOS.x, foodPOS.y, square.NumFood, distanceFromBase));
        if (state == AntState.RandomMovement)
        {
            state = AntState.GoToFood;
            targetPOS = foodPOS;
        }
    }

    private void FindClosestFood()
    {
    }

    public override void North(bool with_food = false)
    {
        currentPOS.y += 1;
        MoveInTurnCounter++;
        base.North(with_food);
    }

    public override void South(bool with_food = false)
    {
        currentPOS.y -= 1;
        MoveInTurnCounter++;
        base.South(with_food);
    }

    public override void East(bool with_food = false)
    {
        currentPOS.x += 1;
        MoveInTurnCounter++;
        base.East(with_food);
    }

    public override void West(bool with_food = false)
    {
        currentPOS.x -= 1;
        MoveInTurnCounter++;
        base.West(with_food);
    }
}