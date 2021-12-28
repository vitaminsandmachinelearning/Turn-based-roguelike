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

    Vector3 boxBasePosition = new Vector3(100, 0, 0);
    public int boxLeftOffset = 120;

    public void BuildTurnOrderList()
    {
        tc = FindObjectOfType<TurnController>();
        for (int i = tc.turnOrder.Count() - 1; i > -1; i--)
            AddTurnOrderBox(tc.turnOrder[i]);
    }

    public void AddTurnOrderBox(Unit u)
    {
        //Create turn order box instance and set as child to turn order panel
        if (turnOrderUIInstances == null)
            turnOrderUIInstances = new List<GameObject>();
        turnOrderUIInstances.Add(Instantiate(turnOrderUIInstance));
        turnOrderUIInstances.Last().transform.SetParent(turnOrderUIPanel.transform, false);
        turnOrderUIInstances.Last().GetComponentInChildren<TurnOrderUIUnitHolder>().unit = u;
    }

    public void RemoveTurnOrderBox(Unit u)
    {
        var instance = turnOrderUIInstances.Find(x => x.GetComponentInChildren<TurnOrderUIUnitHolder>().unit == u);
        Destroy(instance);
        turnOrderUIInstances.Remove(instance);
    }

    public void Next()
    {
        turnOrderUIInstances.Add(turnOrderUIInstances[0]);
        turnOrderUIInstances.RemoveAt(0);
        turnOrderUIInstances.Last().transform.SetAsLastSibling();
    }
}
