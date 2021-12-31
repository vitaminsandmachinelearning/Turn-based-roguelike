using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propagate : SpellComponent
{
    public Unit previous;

    public int currentPropagations = 0;
    public int maxPropagations = 3;
    public float propagationDelay = 0.25f;

    List<Vector2> directions = new List<Vector2> { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

    public override EffectPriority Getpriority() { return EffectPriority.OnHit; }

    public override IEnumerator Effect()
    {
        if (currentPropagations <= maxPropagations && GetComponent<Spell>().target.GetComponent<Unit>() != null)
        {
            //Select all directions with an adjacent unit
            Unit[] units = FindObjectsOfType<Unit>();
            List<Unit> potentialUnits = new List<Unit>();
            foreach (Vector2 v in directions)
            {
                GraphNode nearestNodeInDirection = AstarPath.active.GetNearest((Vector2)transform.position + v).node;
                foreach (Unit u in units)
                {
                    GraphNode nearestNodeToUnit = AstarPath.active.GetNearest(u.transform.position).node;
                    if (nearestNodeInDirection == nearestNodeToUnit)
                        potentialUnits.Add(u);
                }
            }

            //Can't target previous unit
            if (previous != null)
                potentialUnits.Remove(previous);

            //Can't target current target again
            potentialUnits.Remove(GetComponent<Spell>().target.GetComponent<Unit>());

            if (potentialUnits.Count > 0)
            {
                StartCoroutine(PropagateWithDelay(potentialUnits));
            }
        }
        yield return null;
    }

    IEnumerator PropagateWithDelay(List<Unit> potentialUnits)
    {
        yield return new WaitForSeconds(propagationDelay);

        var chosen = potentialUnits[Random.Range(0, potentialUnits.Count)];
        Debug.Log("Propagating to " + chosen.name);
        var prop = Instantiate(gameObject, chosen.transform.position, Quaternion.identity);
        prop.GetComponent<Propagate>().previous = GetComponent<Spell>().target.GetComponent<Unit>();
        prop.GetComponent<Propagate>().currentPropagations++;
        prop.GetComponent<Spell>().target = null;
        prop.GetComponent<Spell>().targetType = TargetType.Position;
        prop.GetComponent<Spell>().Cast();
    }
}