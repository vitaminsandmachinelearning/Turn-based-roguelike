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

    public bool combat = false;

    public int currentTurn = -1;

    private void Update()
    {
        TurnPointer.transform.position = turnOrder[currentTurn].transform.position + turnPointerOffset;
    }

    IEnumerator ProcessCombat()
    {
        while (combat)
        {
            currentTurn = -1;
            foreach (Unit u in turnOrder)
            {
                //Make sure not to run for unit if it died in current loop
                currentTurn++;
                if (u.Alive)
                {
                    GetComponent<TurnOrderUI>().HighlightUnit(u);
                    yield return StartCoroutine(UnitTurn(turnOrder[currentTurn]));
                }
            }
        }
    }

    IEnumerator UnitTurn(Unit u)
    {
        ResetUnitStats(u);
        TurnStartEffects(u);

        //If a unit has the enemy component and their turn has not been ended by TurnStartEffects, start their turn AI
        if (u.GetComponent<Enemy>() != null && u.processingTurnActions)
            u.GetComponent<Enemy>().TakeTurn();

        //Wait for unit to finish taking its turn
        while (u.processingTurnActions)
            yield return null;

        //Process turn end effects, mark turn as not active, increase currentTurn count so next unit can start turn
        TurnEndEffects(u);
    }

    void TurnStartEffects(Unit u)
    {
        u.processingTurnActions = true;
        if (u.GetComponent<StatusEffects>() != null)
        {
            StatusEffects s = u.GetComponent<StatusEffects>();
            if (s.freezeDuration > 0)
            {
                s.freezeDuration--;
                u.processingTurnActions = false;
            }
            s.UpdateStatusIndicators();
        }
    }

    void TurnEndEffects(Unit u)
    {
        if (u.GetComponent<StatusEffects>() != null)
        {
            StatusEffects s = u.GetComponent<StatusEffects>();
            if (s.PoisonStacks().Count > 0)
                foreach (int[] p in s.PoisonStacks())
                {
                    u.TakeDamage(p[1], DamageType.Poison);
                    p[0]--;
                }
            s.UpdateStatusIndicators();
        }
        Util.UpdatePlayerUI();
    }

    void ResetUnitStats(Unit u)
    {
        u.ResetStats();
        Util.UpdatePlayerUI();
    }

    void BuildTurnOrderList()
    {
        units = FindObjectsOfType<Unit>();

        //Roll initiative for all new units
        foreach (Unit u in units)
        {
            if (u.Initiative == -1)
                u.Initiative = Random.Range(0, 20);
            if (u.name.Equals("Player"))
                u.Initiative = 21;
        }

        //ORDER ASCENDING BECAUSE TURNS USE LAST UNIT ON LIST
        SortTurnOrder();

        GetComponent<TurnOrderUI>().BuildTurnOrderList();
    }

    void SortTurnOrder()
    {
        turnOrder = units.OrderByDescending(x => x.Initiative).ToList();
    }

    public void StartCombat()
    {
        units = FindObjectsOfType<Unit>();
        foreach (Unit u in units)
            u.Initiative = -1;
        BuildTurnOrderList();
        combat = true;
        StartCoroutine(ProcessCombat());
    }
}