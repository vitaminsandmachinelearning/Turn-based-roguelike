using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static List<GraphNode> NodesInRange(Vector2 position, int range)
    {
            
        GraphNode start = AstarPath.active.GetNearest(position).node;
        if (range != 0)
            return PathUtilities.BFS(start, range);
        else
            return new List<GraphNode> { start };
    }

    public static GraphNode NearestToCursor()
    { 
        return AstarPath.active.GetNearest(Camera.main.ScreenToWorldPoint(Input.mousePosition)).node;
    }

    public static void UpdatePlayerUI()
    {
        GameObject.FindObjectOfType<PlayerUI>().UpdateUI();
    }
}
