using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public bool Alive = true;
    public int Initiative = -1;
    public int MovementPointsCap = 5;
    public int MovementPoints = 3;
    public int MovementPointsRemaining;
    public int ManaPointsCap = 5;
    public int ManaPoints = 1;
    public int ManaPointsRemaining;
    public bool processingTurnActions;

    public Sprite unitIcon;

    public void TakeDamage(int damage)
    {
        if (Alive)
        {
            if (GetComponent<StatusEffects>() != null)
            {
                StatusEffects s = GetComponent<StatusEffects>();
                if (s.shockPercentage > 0)
                    damage = (int)(damage * (1f + s.shockPercentage / 100f));
            }
            Health -= damage;
            if (Health <= 0)
                Die();
        }
    }

    public void Die()
    {
        Alive = false;
        Debug.Log(name + " is dead.");
    }

    public void ResetStats()
    {
        ManaPointsRemaining = ManaPoints;
        MovementPointsRemaining = MovementPoints;
    }
}
