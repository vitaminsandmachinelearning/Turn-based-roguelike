using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Unit unit;
    public float turnDelay = 0.5f;
    public GameObject SpellPrefab;
    Spell spell;
    GameObject player;

    private void Start()
    {
        unit = GetComponent<Unit>();
        spell = SpellPrefab.GetComponent<Spell>();
        player = GameObject.Find("Player");
    }
    public void TakeTurn()
    {
        //placeholder
        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        bool moved = false;
        while (!moved && unit.ManaPointsRemaining > 0)
        { 
            
        }
        yield return new WaitForSeconds(turnDelay);
        unit.processingTurnActions = false;
    }
}