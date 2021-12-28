using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propagate : MonoBehaviour
{
    public Unit previous;

    public int currentPropagations = 0;
    public int maxPropagations = 3;
    public float propagationDelay = 0f;

    List<Vector2> directions = new List<Vector2> { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

    private void Awake()
    {
        SendMessage("Register");
    }
    void OnHit()
    {
        if (currentPropagations >= maxPropagations)
        {
            SendMessage("Finished");
            return;
        }

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

        //Can't target previously transformed units if transforming
        if (GetComponent<TransformUnit>() != null)
        {
            List<Unit> transformed = new List<Unit>();
            foreach (Unit u in potentialUnits)
                if (u.GetComponent<StatusEffects>() != null)
                    if (u.GetComponent<StatusEffects>().transformed != null)
                        transformed.Add(u);

            potentialUnits.RemoveAll(x => transformed.Contains(x));
        }

        //Can't target current target again
        potentialUnits.Remove(GetComponent<Spell>().target.GetComponent<Unit>());

        if (potentialUnits.Count > 0)
        {
            StartCoroutine(PropagateWithDelay(potentialUnits));
        }
        else
            Debug.Log("No propagation targets");
        SendMessage("Finished");
    }

    IEnumerator PropagateWithDelay(List<Unit> potentialUnits)
    {
        yield return new WaitForSeconds(propagationDelay);

        var chosen = potentialUnits[Random.Range(0, potentialUnits.Count)];
        Debug.Log("Propagating to " + chosen.name);
        var prop = Instantiate(gameObject, chosen.transform.position, Quaternion.identity);
        prop.GetComponent<Propagate>().previous = GetComponent<Spell>().target.GetComponent<Unit>();
        prop.GetComponent<Propagate>().currentPropagations++;
        prop.GetComponent<Spell>().targetType = TargetType.Propagated;
        prop.SendMessage("OnCast");
    }
}