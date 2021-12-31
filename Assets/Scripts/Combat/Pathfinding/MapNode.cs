using System;
using UnityEngine;

public class MapNode
{
    //Values
    public Vector2 position = Vector2.zero;
    public bool blocked = false;

    //Pathfinding
    public float h;
    public float g;
    public float f;
    public MapNode previousInSearch;

    //Debugging
    public int timesChecked = 0;
    public GameObject nodeHighlight;

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

    public void SetHighlight(Sprite sprite)
    {
        nodeHighlight.GetComponent<SpriteRenderer>().sprite = sprite;
        nodeHighlight.transform.position = position;
    }
}
