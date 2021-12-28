using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;

    private void Awake()
    {
        SendMessage("Register");
    }

    void OnHit()
    {
        Debug.Log("Dealing " + damage + " damage to " + GetComponent<Spell>().target.GetComponent<Unit>().name);
        GetComponent<Spell>().target.GetComponent<Unit>().TakeDamage(damage);
        SendMessage("Finished");
    }
}