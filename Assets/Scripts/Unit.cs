using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health;
    public int Initiative = -1;
    public int MovementPoints;
    public int MovementRemaining;
    public int ActionPoints;
    public int ActionPointsRemaining;
    public bool ActiveTurn;

    public Sprite unitIcon;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    public void Die()
    {
        if (name.Equals("Player"))
            Debug.Log("You ded");
        Destroy(gameObject);
    }
}
