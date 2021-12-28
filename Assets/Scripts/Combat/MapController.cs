using Assets.Scripts;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    List<GameObject> activeHighlights;
    public List<GameObject> highlights;
    public Vector3 offsetFromCharacters;
    InputController ic;
    SpellBook sb;
    TurnController tc;

    public GameObject FrozenIndicator;
    public GameObject PoisonIndicator;
    public GameObject TransformIndicator;

    private void Start()
    {
        activeHighlights = new List<GameObject>();
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
                activeHighlights.Add(Instantiate(highlights[1], (Vector3)n.position + offsetFromCharacters, Quaternion.identity));
        }           
    }

    public void ClearHighlighteds()
    {
        foreach (GameObject go in activeHighlights)
            Destroy(go);
        activeHighlights.Clear();

        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit u in units)
        {
            u.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0f);
        }
    }

    //Highlight targetting
    void Update()
    {
        if(!ic.movementRangeHighlighted && sb.currentSpell == null && !tc.hoveredUIUnit)
            ClearHighlighteds();
        if (sb.currentSpell != null)
        {
            ClearHighlighteds();

            int radius = 0;
            if (sb.currentSpell.GetComponent<AoE>() != null)
                radius = sb.currentSpell.GetComponent<AoE>().radius;

            List<GraphNode> nodesToHighlight = new List<GraphNode>();
            if (radius > 0)
                for (int x = -radius; x <= radius; x++)
                    for (int y = -radius; y <= radius; y++)
                    {
                        var n = AstarPath.active.GetNearest(ic.mousePosition + new Vector2(x, y)).node;
                        nodesToHighlight.Add(n);
                    }
            else
                nodesToHighlight = Util.NodesInRange(ic.mousePosition, radius);
            nodesToHighlight = nodesToHighlight.Distinct().ToList();

            Unit[] units = FindObjectsOfType<Unit>();
            foreach (Unit u in units)
            {
                var unitNode = AstarPath.active.GetNearest(u.transform.position).node;
                if (nodesToHighlight.Contains(unitNode))
                {
                    u.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0.75f);
                    u.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
                }
            }


            foreach (GraphNode n in nodesToHighlight)
            {
                activeHighlights.Add(Instantiate(highlights[1], (Vector3)n.position + offsetFromCharacters, Quaternion.identity));
            }
        }
    }
}
