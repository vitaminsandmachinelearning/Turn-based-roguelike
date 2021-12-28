using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    Unit[] units;
    public List<Unit> turnOrder;

    public GameObject TurnPointer;
    public Vector3 turnPointerOffset;
    public bool hoveredUIUnit;

    public bool Combat = true;
    
    private void Update()
    {
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

        GetComponent<TurnOrderUI>().BuildTurnOrderList();
    }

    void SortTurnOrder()
    {
        turnOrder = units.OrderBy(x => x.Initiative).ToList();
    }

    void ResetUnitStats(Unit u)
    {
        u.ManaPointsRemaining = u.ManaPoints;
        u.MovementPointsRemaining = u.MovementPoints;
        Util.UpdatePlayerUI();
    }

    IEnumerator StartRound()
    {
        while (Combat)
        {
            Unit current = turnOrder.Last();
            //Wait if turn still going
            if (current.ActiveTurn)
            {
                Debug.Log("turn is active");
                if (current.name.Equals("Player"))
                {
                    if (current.MovementPointsRemaining <= 0 && current.ManaPointsRemaining <= 0)
                        current.ActiveTurn = false;
                }
                else
                {
                    Debug.Log("waiting turn end");
                    yield return null;
                }
            }
            else
            {
                Debug.Log("turn is ended");
                //End turn and rotate list when turn finished
                if (current.gameObject.activeSelf)
                    TurnEndEffects(current);
                turnOrder.RemoveAll(x => x == null);
                MoveToFront(current);
                //Start next turn if next object in list is active
                if (current.gameObject.activeSelf)
                    StartNextTurn(current);
            }
        }
    }

    void MoveToFront(Unit u)
    {
        Debug.Log("moving " + u.name);
        u = turnOrder.Find(x => x == u);
        turnOrder.Remove(u);
        turnOrder.Insert(0, u);
        GetComponent<TurnOrderUI>().MoveToFront(u);
    }

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

    void StartNextTurn(Unit u)
    {
        if (!u.name.Equals("Player"))
            Debug.Log("<b><color=red>" + u.name + " taking turn. " + u.Initiative + "</color></b>");
        else
            Debug.Log("<b><color=green>" + u.name + " taking turn. </color></b>");

        if (!u.gameObject.activeSelf)
            u.ActiveTurn = false;

        //Swap from u to unit in case of units transforming back
        ResetUnitStats(u);
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
        Util.UpdatePlayerUI();
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
        Util.UpdatePlayerUI();
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
        StartCoroutine(StartRound());
    }
}
