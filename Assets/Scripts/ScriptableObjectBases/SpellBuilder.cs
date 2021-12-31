using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellBuilder
{
    public static GameObject Build(BaseSpell baseSpell)
    {
        GameObject spellObject = new GameObject();
        spellObject.name = baseSpell.Name;

        SpriteRenderer sr = spellObject.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Spells";

        Animator anim = spellObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = baseSpell.animatorController;

        Spell spell = spellObject.AddComponent<Spell>();
        spell.targetType = baseSpell.targetType;
        spell.castRange = baseSpell.castRange;

        if (baseSpell.Damage)
        {
            Damage damage = spellObject.AddComponent<Damage>();
            damage.damage = baseSpell.damage;
            damage.damageType = baseSpell.damageType;
        }

        if (baseSpell.AoE)
        {
            AoE aoe = spellObject.AddComponent<AoE>();
            aoe.radius = baseSpell.radius;
        }

        if (baseSpell.ApplyStatusEffects)
        {
            ApplyStatus applyStatus = spellObject.AddComponent<ApplyStatus>();
            applyStatus.poisonDamage = baseSpell.poisonDamage;
            applyStatus.poisonDuration = baseSpell.poisonDuration;
            applyStatus.freezeDuration = baseSpell.freezeDuration;
            applyStatus.shockPercentage = baseSpell.shockPercentage;
        }

        if (baseSpell.Propagation)
        {
            Propagate propagate = spellObject.AddComponent<Propagate>();
            propagate.maxPropagations = baseSpell.maxPropagations;
            propagate.propagationDelay = baseSpell.propagationDelay;
        }
        return spellObject;
    }
}