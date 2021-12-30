using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    List<GameObject> activeHighlights;
    List<GameObject> activeTargetHighlights;
    List<GameObject> activeUnitMovementHighlights;

    public List<GameObject> highlights;

    InputController ic;
    SpellBook sb;
    TurnController tc;

    public GameObject UnitHovered;

    private void Start()
    {
        activeHighlights = new List<GameObject>();
        activeTargetHighlights = new List<GameObject>();
        activeUnitMovementHighlights = new List<GameObject>();
        ic = FindObjectOfType<InputController>();
        sb = FindObjectOfType<SpellBook>();
        tc = FindObjectOfType<TurnController>();
    }
    public void HighlightMovementRange(Vector2 position, int range)
    {
        ClearHighlighteds();
        GraphNode start = AstarPath.active.GetNearest(position).node;
        List<Unit> units = new List<Unit>(FindObjectsOfType<Unit>());
        List<GraphNode> nodesNearEnemies = new List<GraphNode>();
        foreach (Unit u in units)
        {
            if (u.GetComponent<Enemy>())
                nodesNearEnemies.Add(AstarPath.active.GetNearest(u.transform.position).node);
        }
        List<GraphNode> nodesInRange = Util.NodesInRange(position, range);
        foreach (GraphNode n in nodesInRange)
        { 
            if (n != start && !nodesNearEnemies.Contains(n))
                activeHighlights.Add(Instantiate(highlights[1], (Vector3)n.position, Quaternion.identity));
        }           
    }

    public void ClearHighlighteds()
    {
        foreach (GameObject go in activeHighlights)
            Destroy(go);
        activeHighlights.Clear();

        foreach (GameObject go in activeTargetHighlights)
            Destroy(go);
        activeTargetHighlights.Clear();

        foreach (GameObject go in activeUnitMovementHighlights)
            Destroy(go);
        activeUnitMovementHighlights.Clear();

        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit u in units)
        {
            u.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0f);
        }
    }

    //Highlight targetting
    void Update()
    {
        //Movement range and spell targetting
        if(!ic.movementRangeHighlighted && sb.currentSpell == null && !tc.hoveredUIUnit)
            ClearHighlighteds();
        if (sb.currentSpell != null)
        {
            ClearHighlighteds();

            int radius = 0;
            if (sb.currentSpell.GetComponent<AoE>() != null)
                radius = sb.currentSpell.GetComponent<AoE>().radius;

            List<GraphNode> rangeNodesToHighlight = new List<GraphNode>();
            List<GraphNode> targetNodesToHighlight = new List<GraphNode>();

            rangeNodesToHighlight = Util.NodesInRange(ic.transform.position, sb.currentSpell.GetComponent<Spell>().castRange);

            if (radius > 0)
                for (int x = -radius; x <= radius; x++)
                    for (int y = -radius; y <= radius; y++)
                    {
                        var n = AstarPath.active.GetNearest(ic.mousePosition + new Vector2(x, y)).node;
                        targetNodesToHighlight.Add(n);
                    }
            else
                targetNodesToHighlight = Util.NodesInRange(ic.mousePosition, radius);
            targetNodesToHighlight = targetNodesToHighlight.Distinct().ToList();

            Unit[] units = FindObjectsOfType<Unit>();
            foreach (Unit u in units)
            {
                var unitNode = AstarPath.active.GetNearest(u.transform.position).node;
                if (targetNodesToHighlight.Contains(unitNode))
                {
                    u.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0.75f);
                    u.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
                }
            }

            foreach (GraphNode n in rangeNodesToHighlight)
                activeHighlights.Add(Instantiate(highlights[1], (Vector3)n.position, Quaternion.identity));
            foreach (GraphNode n in targetNodesToHighlight)
                activeTargetHighlights.Add(Instantiate(highlights[2], (Vector3)n.position, Quaternion.identity));
        }

        //Hovered units
        if (UnitHovered != null && !ic.movementRangeHighlighted && sb.currentSpell == null)
        {
            ClearHighlighteds();
            List<GraphNode> nodesInRangeOfUnit = Util.NodesInRange(UnitHovered.transform.position, UnitHovered.GetComponent<Unit>().MovementPoints);
            foreach (GraphNode n in nodesInRangeOfUnit)
                activeUnitMovementHighlights.Add(Instantiate(highlights[3], (Vector3)n.position, Quaternion.identity));
        }
    }
}
