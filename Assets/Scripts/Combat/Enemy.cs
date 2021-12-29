using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Unit unit;
    public float turnDelay = 0.5f;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }
    public void TakeTurn()
    {
        //placeholder
        StartCoroutine(DelayTurn());
    }

    IEnumerator DelayTurn()
    {
        yield return new WaitForSeconds(turnDelay);
        unit.processingTurnActions = false;
    }
}