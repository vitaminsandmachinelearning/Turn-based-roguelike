using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPath
{
    public List<MapNode> path;

    public MapPath()
    {
        path = new List<MapNode>();
    }
    public int Length()
    {
        return path.Count();
    }

    public void Add(MapNode node)
    {
        path.Add(node);
    }

    public void Insert(int index, MapNode node)
    {
        path.Insert(index, node);
    }

    public bool Contains(MapNode node)
    {
        return path.Contains(node);
    }
}
