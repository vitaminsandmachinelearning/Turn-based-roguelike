using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool canInput = true;
    public bool movementInput = false;
    public Vector2 mousePosition;
    MapController mp;
    TurnController tc;
    Unit player;

    public bool movementRangeHighlighted = false;

    private void Start()
    {
        mp = FindObjectOfType<MapController>();
        tc = FindObjectOfType<TurnController>();
        player = GameObject.Find("Player").GetComponent<Unit>();
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (canInput)
        {
            if (player.ActiveTurn)
            {
                //Movement controls
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    FindObjectOfType<SpellBook>().currentSpell = null;
                    movementRangeHighlighted = true;
                    mp.HighlightMovementRange(player.transform.position, player.MovementRemaining);
                }
                if (Input.GetMouseButtonDown(0) && !movementInput && movementRangeHighlighted)
                {
                    movementInput = true;
                    movementRangeHighlighted = false;
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
                    player.ActiveTurn = false;
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


    public void FinishMoving()
    {
        canInput = true;
        movementInput = false;
        mousePosition = transform.position;
    }
}
