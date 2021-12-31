using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{ 
    Physical,   //Higher base damage
    Lightning,  //Scales with shock
    Ice,        //+100% damage to frozen targets
    Poison      //Stacks infinitely
}

public class Damage : SpellComponent
{
    public int damage = 0;
    public DamageType damageType = DamageType.Physical;

    public override EffectPriority Getpriority() { return EffectPriority.OnHit; }
    public override IEnumerator Effect()
    {
        GetComponent<Spell>().target.GetComponent<Unit>().TakeDamage(damage, damageType);
        yield return null;
    }
}