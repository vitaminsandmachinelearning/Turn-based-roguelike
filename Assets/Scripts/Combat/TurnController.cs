using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    int roundCount = 0;
    bool newRound = false;
    Unit[] units;
    public List<Unit> turnOrder;

    public GameObject TurnPointer;
    public Vector3 turnPointerOffset;
    public bool hoveredUIUnit;

    public bool Combat = true;
    
    private void Update()
    {
        if (newRound)
        {
            newRound = false;
            NewRound();
        }
        TurnPointer.transform.position = turnOrder.Last().transform.position + turnPointerOffset;
    }

    void BuildTurnOrderList()
    {
        units = FindObjectsOfType<Unit>();

        //Roll initiative for all new units
        foreach (Unit u in units)
            if (u.Initiative == -1)
                u.Initiative = Random.Range(0, 20);

        //ORDER ASCENDING BECAUSE TURNS USE LAST UNIT ON LIST
        SortTurnOrder();

        turnOrder.Last().ActiveTurn = true;

        GetComponent<TurnOrderUI>().BuildTurnOrderList();
    }

    void SortTurnOrder()
    {
        turnOrder = units.OrderBy(x => x.Initiative).ToList();
    }

    void ResetUnitStats()
    {
        foreach (Unit u in units)
        {
            u.ActionPointsRemaining = u.ActionPoints;
            u.MovementRemaining = u.MovementPoints;
        }
    }

    void NewRound()
    {
        ResetUnitStats();
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        //start first turn before loop starts
        if (turnOrder.Last().gameObject.activeSelf)
            StartNextTurn(turnOrder.Last());
        while (Combat)
        {
            //Wait if turn still going
            if (turnOrder.Last().ActiveTurn)
                yield return null;
            else
            {
                //End turn and rotate list when turn finished
                if (turnOrder.Last().gameObject.activeSelf)
                    TurnEndEffects(turnOrder.Last());
                turnOrder.RemoveAll(x => x == null);
                MoveToFront();
                //Start next turn if next object in list is active
                if (turnOrder.Last().gameObject.activeSelf)
                    StartNextTurn(turnOrder.Last());
            }
        }
        roundCount++;
        newRound = true;
    }
    void StartNextTurn(Unit u)
    {    
        //if(!u.name.Equals("Player"))
        //    Debug.Log("<b><color=red>" + u.name + " taking turn. "+u.Initiative+"</color></b>");
        //else
        //    Debug.Log("<b><color=green>" + u.name + " taking turn. </color></b>");

        //Swap from u to unit in case of units transforming back

        var unit = TurnStartEffects(u);
        unit.ActiveTurn = true;

        //Start enemy AI turn or skip player turn if player is transformed
        if (unit.GetComponent<Enemy>() != null)
            unit.GetComponent<Enemy>().TakeTurn();
        else if (unit.name.Equals("Player"))
            if (unit.GetComponent<StatusEffects>() != null)
                if (unit.GetComponent<StatusEffects>().transformed != null)
                    unit.ActiveTurn = false;
    }

    void MoveToFront()
    {
        turnOrder.Insert(0, turnOrder.Last());
        turnOrder.RemoveAt(turnOrder.Count - 1);
        GetComponent<TurnOrderUI>().Next();
    }

    //if insert is true, re-sort list
    public void InsertTransformedUnit(Unit u, bool atEnd)
    {

        if (atEnd)
            turnOrder.Add(u);
        else
        {
            int nextInQueueByInitiative = turnOrder.FindIndex(x => x.Initiative >= u.Initiative);
            turnOrder.Insert(nextInQueueByInitiative, u);
        }
        GetComponent<TurnOrderUI>().AddTurnOrderBox(u);
    }

    Unit TurnStartEffects(Unit u)
    {
        if (u.GetComponent<StatusEffects>() != null)
        {
            StatusEffects s = u.GetComponent<StatusEffects>();
            if (s.transformed != null)
            {
                s.transformDuration--;
                if (s.transformDuration <= 0)
                {
                    GetComponent<TurnOrderUI>().RemoveTurnOrderBox(u);
                    u = s.transformed.GetComponent<Unit>();
                    GetComponent<TurnOrderUI>().AddTurnOrderBox(u);
                    u.ActiveTurn = true;
                    s.TransformBack();
                }
            }
            if (s.freezeDuration > 0)
            {
                s.freezeDuration--;
                u.ActiveTurn = false;
            }
            else if (u.GetComponent<Enemy>() != null)
                u.GetComponent<Enemy>().TakeTurn();
            s.UpdateStatusIndicators();
        }
        return u;
    }

    void TurnEndEffects(Unit u)
    {
        if (u.GetComponent<StatusEffects>() != null)
        {
            StatusEffects s = u.GetComponent<StatusEffects>();
            if (s.PoisonStacks().Count > 0)
                foreach (int[] p in s.PoisonStacks())
                {
                    u.TakeDamage(p[1]);
                    p[0]--;
                }
            s.UpdateStatusIndicators();
        }
    }

    public void DEBUG_STARTCOMBAT()
    {
        StartCombat();
    }

    void StartCombat()
    {
        units = FindObjectsOfType<Unit>();
        foreach (Unit u in units)
            u.Initiative = -1;
        BuildTurnOrderList();
        NewRound();
    }
}
