using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using UnityEngine.Assertions;

public class AoE : SpellComponent
{
    public int radius;

    public override EffectPriority Getpriority(){ return EffectPriority.PostCast; }

    public override IEnumerator Effect()
    {
        if (radius > 0 && GetComponent<Spell>().targetType != TargetType.Position)
            if (GetComponent<Propagate>() != null)
            {
                if (GetComponent<Propagate>().currentPropagations == 0)
                    CastInAoe();
            }
            else
                CastInAoe();
        yield return null;
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
                o.GetComponent<Spell>().target = null;
                o.GetComponent<Spell>().targetType = TargetType.Position;
            }
        }
    }
}
