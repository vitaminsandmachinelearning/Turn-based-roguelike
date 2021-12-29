using Assets.Scripts;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool canInput = true;
    public Vector2 mousePosition;
    VisionController mp;
    TurnController tc;
    Unit unit;

    public bool movementRangeHighlighted = false;

    private void Start()
    {
        mp = FindObjectOfType<VisionController>();
        tc = FindObjectOfType<TurnController>();
        unit = GameObject.Find("Player").GetComponent<Unit>();
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (canInput)
        {
            if (unit.processingTurnActions)
            {
                //Movement controls
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    FindObjectOfType<SpellBook>().currentSpell = null;
                    movementRangeHighlighted = true;
                    mp.HighlightMovementRange(unit.transform.position, unit.MovementPointsRemaining);
                }
                if (Input.GetMouseButtonDown(0) && movementRangeHighlighted)
                {
                    StartMovement();
                }

                //casting controls
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    GetComponent<SpellBook>().SelectSpell(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    GetComponent<SpellBook>().SelectSpell(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GetComponent<SpellBook>().SelectSpell(2);
                }
                if (Input.GetMouseButtonDown(0) && !movementRangeHighlighted)
                {
                    if (GetComponent<SpellBook>().currentSpell != null)
                        GetComponent<SpellBook>().CastSpell();
                }

                //Turn controls
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    unit.processingTurnActions = false;
                }
            }
            //Interface controls
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                FindObjectOfType<SpellBook>().currentSpell = null;
                movementRangeHighlighted = false;
            }
        }
    }

    void StartMovement()
    {
        movementRangeHighlighted = false;
        GraphNode nearestToMouseClick = AstarPath.active.GetNearest(mousePosition).node;
        if (Util.NodesInRange(unit.transform.position, unit.MovementPointsRemaining).Contains(nearestToMouseClick))
        {
            //Can't move onto squares with units already occupying
            List<Unit> units = new List<Unit>(FindObjectsOfType<Unit>());
            foreach (Unit u in units)
            {
                if (AstarPath.active.GetNearest(u.transform.position).node.Equals(nearestToMouseClick))
                    return;
            }
            canInput = false;
            GetComponent<TurnBasedMovementAI>().StartMovementCalculation(mousePosition);
        }
    }

    public void FinishedMoving()
    {
        canInput = true;
        mousePosition = transform.position;
    }

    public void StartedMoving()
    {
        Util.UpdatePlayerUI();
    }
}
