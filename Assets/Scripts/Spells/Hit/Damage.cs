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
        GetComponent<Spell>().target.GetComponent<Unit>().TakeDamage(damage);
        SendMessage("Finished");
    }
}