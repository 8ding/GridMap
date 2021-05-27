using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private MyGrid<PathNode> grid;
    private List<PathNode> opentList;
    private List<PathNode> closeList;
    public Pathfinding(int width, int height)
    {
        grid = new MyGrid<PathNode>(width, height, 10f, Vector3.zero, ((myGrid, x, y) => new PathNode(myGrid, x, y)));
    }
    // private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    // {
    //     PathNode startNode = grid.GetGridObject(startX, startY);
    //     PathNode endNode = grid.GetGridObject(endX, endY);
    //     opentList = new List<PathNode> {startNode};
    //     closeList = new List<PathNode> {};
    //     for (int x = 0; x < grid.GetWidth(); x++)
    //     {
    //         for (int y = 0; y < grid.GetHeight(); y++)
    //         {
    //             PathNode pathNode = grid.GetGridObject(x, y);
    //             pathNode.gCost = int.MaxValue;
    //             pathNode.CalculateFCost();
    //             pathNode.CamePathNode = null;
    //         }
    //     }
    // }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}
