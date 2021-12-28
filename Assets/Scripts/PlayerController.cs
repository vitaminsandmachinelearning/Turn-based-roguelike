using Pathfinding;
using Assets.Scripts;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    InputController ic;
    MapController mp;
    Unit unit;

    AIMoveTarget aiMoveTarget;
    Seeker s;
    
    public GameObject MoveTarget;
    BlockManager blockManager;
    public List<SingleNodeBlocker> obstacles;
    BlockManager.TraversalProvider traversalProvider;

    void Start()
    {
        ic = GetComponent<InputController>();
        mp = FindObjectOfType<MapController>();
        unit = GetComponent<Unit>();

        aiMoveTarget = GetComponent<AIMoveTarget>();
        s = GetComponent<Seeker>();

        blockManager = FindObjectOfType<BlockManager>();
        obstacles = new List<SingleNodeBlocker>() { GetComponent<SingleNodeBlocker>() };
        traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.AllExceptSelector, obstacles);
    }

    void Update()
    {
        if (unit.ActiveTurn)
            if (ic.movementInput)
            {
                ic.movementInput = false;
                GraphNode nearestToMouseClick = AstarPath.active.GetNearest(ic.mousePosition).node;
                if (Util.NodesInRange(unit.transform.position, unit.MovementRemaining).Contains(nearestToMouseClick))
                {
                    //Can't move onto squares with units already occupying
                    List<Unit> units = new List<Unit>(FindObjectsOfType<Unit>());
                    foreach(Unit u in units)
                    {
                        if (AstarPath.active.GetNearest(u.transform.position).node.Equals(nearestToMouseClick))
                        {
                            ic.movementRangeHighlighted = false;
                            return;
                        }
                    }

                    MoveTarget.transform.position = (Vector3)nearestToMouseClick.position;
                    var p = ABPath.Construct(transform.position, ic.mousePosition, Move);
                    p.traversalProvider = traversalProvider;
                    AstarPath.StartPath(p);
                    if (p.error)
                    {
                        ic.movementRangeHighlighted = false;
                        return;
                    }
                    ic.canInput = false;
                }
            }
    }

    public void Move(Path p)
    {
        ic.canInput = true;
        //if (p.path.Count - 1 <= unit.MovementRemaining)
        {
            unit.MovementRemaining -= p.path.Count - 1;
            ic.movementRangeHighlighted = false;
            aiMoveTarget.enabled = true;
        }
    }
}