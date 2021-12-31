using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Unit unit;
    public float turnDelay = 0.25f;
    public BaseSpell spell;
    GameObject player;

    public bool finishedMoving = false;

    private void Start()
    {
        unit = GetComponent<Unit>();
        player = GameObject.Find("Player");
    }
    public void TakeTurn()
    {
        //placeholder
        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        GraphNode playerNode = AstarPath.active.GetNearest(player.transform.position).node;

        while (!Util.NodesInRange(transform.position, spell.castRange).Contains(playerNode) && unit.MovementPointsRemaining > 0)
        {
            finishedMoving = false;
            Vector2 direction = player.transform.position - transform.position;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                GetComponent<TurnBasedMovementAI>().StartMovementCalculation(transform.position + new Vector3((int)Mathf.Clamp(direction.x, -1, 1), 0, 0));
            }
            else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                GetComponent<TurnBasedMovementAI>().StartMovementCalculation(transform.position + new Vector3(0, (int)Mathf.Clamp(direction.y, -1, 1), 0));
            }
            else if (direction.magnitude != 0)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                    GetComponent<TurnBasedMovementAI>().StartMovementCalculation(transform.position + new Vector3((int)Mathf.Clamp(direction.x, -1, 1), 0, 0));
                else
                    GetComponent<TurnBasedMovementAI>().StartMovementCalculation(transform.position + new Vector3(0, (int)Mathf.Clamp(direction.y, -1, 1), 0));
            }
            else
                finishedMoving = true;
            while (!finishedMoving)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        
        if (Util.NodesInRange(transform.position, spell.castRange).Contains(playerNode))
            UseSpell(player.transform.position);

        yield return new WaitForSeconds(turnDelay);
        unit.processingTurnActions = false;
    }

    void UseSpell(Vector3 position)
    {
        unit.ManaPointsRemaining--;
        var spellInstance = SpellBuilder.Build(spell);
        spellInstance.transform.position = (Vector3)AstarPath.active.GetNearest(position).node.position;
        spellInstance.GetComponent<Spell>().Cast();
    }

    void FinishedMoving()
    {
        finishedMoving = true;
    }
}