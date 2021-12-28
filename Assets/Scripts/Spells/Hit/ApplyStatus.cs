using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStatus : MonoBehaviour
{
    public int poisonDuration;
    public int poisonDamage;
    public int freezeDuration;

    private void Awake()
    {
        SendMessage("Register");
    }

    void OnHit()
    {
        Debug.Log("Applying status to " + GetComponent<Spell>().target.GetComponent<Unit>().name);
        StatusEffects s;
        if (GetComponent<Spell>().target.GetComponent<StatusEffects>() == null)
            s = GetComponent<Spell>().target.AddComponent<StatusEffects>();
        else
            s = GetComponent<Spell>().target.GetComponent<StatusEffects>();
        if (poisonDuration > 0) s.AddPoison(poisonDuration, poisonDamage);
        if (freezeDuration > 0) s.ApplyFreeze(freezeDuration);
        SendMessage("Finished");
    }
}