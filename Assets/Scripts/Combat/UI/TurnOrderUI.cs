using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderUI : MonoBehaviour
{
    TurnController tc;

    public GameObject turnOrderUIPanel;
    public GameObject turnOrderUIInstance;
    [ShowInInspector]
    List<GameObject> turnOrderUIInstances;

    public int boxLeftOffset = 120;

    public void BuildTurnOrderList()
    {
        tc = FindObjectOfType<TurnController>();
        for (int i = 0; i < tc.turnOrder.Count(); i++)
            AddUnit(tc.turnOrder[i]);
    }

    public void AddUnit(Unit u)
    {
        //Create turn order box instance and set as child to turn order panel
        if (turnOrderUIInstances == null)
            turnOrderUIInstances = new List<GameObject>();
        turnOrderUIInstances.Add(Instantiate(turnOrderUIInstance));
        turnOrderUIInstances.Last().transform.SetParent(turnOrderUIPanel.transform, false);
        turnOrderUIInstances.Last().GetComponentInChildren<TurnOrderUIUnitHolder>().unit = u;
    }

    public void RemoveUnit(Unit u)
    {
        var instance = turnOrderUIInstances.Find(x => u == x.GetComponentInChildren<TurnOrderUIUnitHolder>().unit);
        instance.SetActive(false);
    }

    public void HighlightUnit(Unit u)
    {
        foreach (GameObject go in turnOrderUIInstances)
            go.transform.localScale = Vector3.one;
        var instance = turnOrderUIInstances.Find(x => x.GetComponentInChildren<TurnOrderUIUnitHolder>().unit == u);
        instance.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
    }
}
