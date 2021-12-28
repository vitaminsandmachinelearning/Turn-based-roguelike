using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Pathfinding;
using System.Linq;

public class AoE : MonoBehaviour
{
    public int radius;

    private void Awake()
    {
        SendMessage("Register");
    }

    void OnPostCast()
    {
        if (radius > 0 && GetComponent<Spell>().targetType != TargetType.Position)
            if (GetComponent<Propagate>() != null)
            {
                if (GetComponent<Propagate>().currentPropagations == 0)
                    CastInAoe();
            }
            else
                CastInAoe();
        SendMessage("Finished");
    }

    void CastInAoe()
    {
        //Get all nodes in radius
        List<GraphNode> nodesInRadius = new List<GraphNode>();
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var n = AstarPath.active.GetNearest(transform.position + new Vector3(x, y, 0)).node;
                nodesInRadius.Add(n);
            }

        //Keep only unique nodes, exclude start node
        nodesInRadius = nodesInRadius.Distinct().ToList();
        nodesInRadius.Remove(AstarPath.active.GetNearest(transform.position).node);

        //Spawn new instances on enemies
        foreach (GraphNode n in nodesInRadius)
        {
            if (nodesInRadius.Contains(n))
            {
                var o = Instantiate(gameObject, (Vector3)n.position, Quaternion.identity);
                o.GetComponent<Spell>().targetType = TargetType.Position;
                o.SendMessage("OnCast");
            }
        }
    }
}
