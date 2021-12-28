using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Initiative = -1;
    public int MovementPoints;
    public int MovementPointsRemaining;
    public int ManaPoints;
    public int ManaPointsRemaining;
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
