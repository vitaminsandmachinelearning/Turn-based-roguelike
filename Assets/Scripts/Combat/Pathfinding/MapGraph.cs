using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using System.IO;

public class MapGraph : MonoBehaviour
{
    [BoxGroup("Map")] 
    public int2 StartPosition, Size;

    [BoxGroup("Map")] 
    public MapNode[,] nodes;

    public MapPath LatestPath;

    List<MapNode> blockedNodes = new List<MapNode>();

    void Generate()
    {
        nodes = new MapNode[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
            {
                nodes[x, y] = new MapNode(x + StartPosition.x, y + StartPosition.y);
            }
    }

    void BlockNode(MapNode node)
    {
        blockedNodes.Add(node);
        node.blocked = true;
    }

    void UnblockNode(MapNode node)
    {
        blockedNodes.Remove(node);
        node.blocked = false;
    }

    MapNode GetNearest(Vector2 position)
    {
        foreach (MapNode node in nodes)
            if (node.position == new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)))
                return node;
        return null;
    }

    List<MapNode> GetNeighbors(MapNode node)
    {
        List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        List<MapNode> neighbors = new List<MapNode>();
        foreach (Vector2 direction in directions)
        {
            MapNode nearest = GetNearest(node.position + direction);
            if(nearest != null)
                neighbors.Add(nearest);
        }
        return neighbors;
    }

    IEnumerator FindPathWithPositions(Vector2 start, Vector2 goal)
    {
        MapNode startNode = GetNearest(start);
        MapNode goalNode = GetNearest(goal);
        if (startNode != null && goalNode != null)
            yield return FindPathWithNodes(startNode, goalNode);
    }

    IEnumerator FindPathWithNodes(MapNode start, MapNode goal)
    {
        LatestPath = null;

        // Create open list containing only start node
        List<MapNode> open = new List<MapNode>{ start };
        List<MapNode> closed = new List<MapNode>();

        // Set default g, f values and resetting previousInSearch to null
        // g: cost of path from start to current node
        // f: approximate cost of path from start node to goal node via current node [f = g + h]
        // h: value returned by heuristic algorithm, approximate cost of path from current node to goal node
        foreach (MapNode node in nodes)
        {
            node.g = 1000f;
            node.h = GetHeuristicValue(node, goal);
            node.f = node.g + node.h;
            node.previousInSearch = null;
        }
        start.g = 0;
        start.f = GetHeuristicValue(start, goal);

        //Create current with value of start node
        MapNode current = null;

        while (open.Count() > 0)
        {
            //Sort open list by f value and set current to lowest f value node
            foreach (MapNode node in open)
                node.f = node.g + GetHeuristicValue(node, goal);
            current = open[0];
            for (int i = 0; i < open.Count(); i++)
                if (open[i].f < current.f)
                    current = open[i];

            //If goal is found, break
            if (current.Equals(goal))
            {
                LatestPath = WalkBackPath(current);
                yield break;
            }

            //Remove current from open list as it has been searched
            open.Remove(current);
            closed.Add(current);

            List<MapNode> neighbors = GetNeighbors(current);

            //Check each neighbor
            foreach (MapNode node in neighbors)
            {
                node.timesChecked++;
                if (node.Equals(goal))
                {
                    node.previousInSearch = current;
                    LatestPath = WalkBackPath(node);
                    yield break;
                }
                if (closed.Contains(node))
                    continue;
                if (open.Contains(node))
                {
                    if (node.g < current.g + GetHeuristicValue(current, node))
                        continue;
                }
                else
                    open.Add(node);
                node.previousInSearch = current;
                node.g = current.g + GetHeuristicValue(current, node);
                node.h = GetHeuristicValue(node, goal);
                node.f = node.g + node.h;
                yield return null;
            }
        }
    }

    float GetHeuristicValue(MapNode a, MapNode b)
    {
        return Mathf.Abs(a.position.x - b.position.x) + Mathf.Abs(a.position.y - b.position.y); 
    }

    MapPath WalkBackPath(MapNode node)
    {
        MapPath path = new MapPath();
        path.Add(node);
        while (node.previousInSearch != null)
        {
            node = node.previousInSearch;
            path.Insert(0, node);
        }
        return path;
    }

    //Debugging

    IEnumerator ExportMapAsCSV()
    {
        string filePath = Application.persistentDataPath + "/output.csv";

        if (File.Exists(filePath))
            File.Delete(filePath);

        var output = File.CreateText(filePath);

        string outputText = "A,";

        for (int i = 0; i < nodes.GetLength(0); i++)
            outputText += "x:" + (i - 5) + ",";
        outputText += "\n";

        for (int j = nodes.GetLength(1) - 1; j > -1; j--) 
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                if (i == 0)
                    outputText += "y:" + (j - 3) + ",";
                outputText += nodes[i, j].timesChecked + ",";
            }
            outputText += "\n";
        }

        output.Write(outputText);

        output.Close();

        Debug.Log("Writing to " + filePath);
        yield return new WaitForSeconds(1f);
        Application.OpenURL(filePath);
    }
}
