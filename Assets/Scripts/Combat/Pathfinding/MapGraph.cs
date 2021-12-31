using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
    [BoxGroup("Generate Map")] 
    public int2 StartPosition, Size;

    [BoxGroup("Generate Map")] 
    public MapNode[,] nodes;

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
        node.blocked = true;
    }

    void UnblockNode(MapNode node)
    {
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
        float distanceToGoal = Vector2.Distance(start.position, goal.position);

        // Reset pathfinding values
        foreach (MapNode node in nodes)
        {
            node.visitedInSearch = false;
            node.previousInSearch = null;
        }
        LatestPath = null;

        // Start pathing
        Debug.Log("Started pathing.");
        List<MapNode> queue = new List<MapNode>();
        queue.Add(start);
        MapNode current;

        // While items are in the queue, search for goal
        while (queue.Count() > 0)
        {
            current = queue.Last();
            queue.Remove(current);
            current.visitedInSearch = true;
            Debug.Log("Current node: " + current.ToString());
            if (current.Equals(goal))
                LatestPath = WalkBackPath(current);
            // If current isn't goal, get all unvisited neighbors, add them to the queue if not blocked
            else
            {
                List<MapNode> neighbors = GetNeighbors(current);
                Debug.Log("Neighbors found: " + neighbors.Count());
                Debug.Log("Removed " + neighbors.RemoveAll(x => x.visitedInSearch));
                foreach (MapNode node in neighbors)
                {
                    Debug.Log(string.Format("Checking node: {0}\nVisited: {1}\nBlocked: {2}", node.ToString(), node.visitedInSearch, node.blocked));
                    node.previousInSearch = current;
                    node.visitedInSearch = true;
                    if (node.Equals(goal))
                    {
                        LatestPath = WalkBackPath(node);
                        break;
                    }
                    else if (!node.blocked)
                        queue.Add(node);
                }
            }
            if (LatestPath != null)
                break;
            yield return null;
        }
        Debug.Log("Finished Pathing. Path: " + LatestPath);
    }

    MapPath WalkBackPath(MapNode node)
    {
        MapPath path = new MapPath();
        path.Add(node);
        int count = 0;
        int maxLength = 100;
        while (node.previousInSearch != null && count < maxLength)
        {
            node = node.previousInSearch;
            path.Insert(0, node);
        }
        return path;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            Generate();
        if (Input.GetKeyDown(KeyCode.F2))
            StartCoroutine(FindPathWithPositions(PathStart, PathEnd));
    }

    /// EDITOR
#if UNITY_EDITOR

    [BoxGroup("Pathfinding")] 
    public Vector2 PathStart, PathEnd;

    [ShowInInspector, BoxGroup("Pathfinding")] 
    public static MapPath LatestPath;


    [BoxGroup("Generate Map"), ShowInInspector]
    List<MapNode> nodesToBlock = new List<MapNode>();

    [BoxGroup("Generate Map"), Button("Block Nodes")]
    void BlockNodes()
    {
        foreach (MapNode node in nodesToBlock)
            node.blocked = true;
    }

    /// GIZMOS

    private void OnDrawGizmosSelected()
    {
        if (nodes != null)
        {
            Gizmos.color = Color.white;
            foreach (MapNode node in nodes)
                Gizmos.DrawCube(node.position, new Vector3(0.5f, 0.5f, 0.5f));
        }
        if (LatestPath != null)
        {
            Gizmos.color = Color.green;
            for (int i = 1; i < LatestPath.Length(); i++)
                Gizmos.DrawLine(LatestPath.path[i].position, LatestPath.path[i - 1].position);
        }

        if (LatestPath != null)
            foreach (MapNode node in LatestPath.path)
                Gizmos.DrawSphere(node.position, 0.5f);
    }

    /// ODIN METHODS

    static MapNode DrawNodes(Rect rect, MapNode node)
    {
        Color rectColor = Color.grey;
        rectColor = node.blocked ? Color.red : Color.grey;
        rectColor = node.visitedInSearch ? Color.blue : rectColor;
        if (LatestPath != null)
            rectColor = LatestPath.Contains(node) ? Color.green : rectColor;
        UnityEditor.EditorGUI.DrawRect(rect.Padding(1), rectColor);
        return node;
    }
    
    #endif
}
