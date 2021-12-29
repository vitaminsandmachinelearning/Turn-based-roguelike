using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStatus : MonoBehaviour
{
    public int poisonDuration = 0;
    public int poisonDamage = 0;
    public int freezeDuration = 0;
    public int shockPercentage = 0;

    private void Awake()
    {
        SendMessage("Register");
    }

    void OnHit()
    {
        if (GetComponent<Spell>().target.GetComponent<Unit>().Alive)
        {
            StatusEffects s;
            if (GetComponent<Spell>().target.GetComponent<StatusEffects>() == null)
                s = GetComponent<Spell>().target.AddComponent<StatusEffects>();
            else
                s = GetComponent<Spell>().target.GetComponent<StatusEffects>();
            if (poisonDuration > 0) s.AddPoison(poisonDuration, poisonDamage);
            if (freezeDuration > 0) s.ApplyFreeze(freezeDuration);
            if (shockPercentage > 0) s.ApplyShock(shockPercentage);
        }
        SendMessage("Finished");
    }
}