using System;
using UnityEngine;

public class MapNode
{
    public Vector2 position = Vector2.zero;
    public bool blocked = false;
    public bool visitedInSearch = false;
    public MapNode previousInSearch;

    public MapNode()
    {
        position.x = 0;
        position.y = 0;
        blocked = false;
    }

    public MapNode(int x, int y)
    {
        position.x = x;
        position.y = y;
        blocked = false;
    }

    public bool Equals(MapNode other)
    {
        if (other == null) throw new ArgumentException("Parameter cannot be null", nameof(other));

        return position == other.position;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", position.x, position.y);
    }
}
