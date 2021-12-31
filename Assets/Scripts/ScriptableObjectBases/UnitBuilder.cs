using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1SpriteShader;

public static class UnitBuilder 
{
    public static GameObject Build(BaseUnit baseUnit)
    {
        GameObject unitObject = new GameObject();
        unitObject.name = baseUnit.Name;
        unitObject.layer = 6;

        SpriteRenderer sr = unitObject.AddComponent<SpriteRenderer>();
        sr.sprite = baseUnit.icon;
        sr.material = baseUnit.material;

        Animator anim = unitObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = baseUnit.animatorController;

        Unit unit = unitObject.AddComponent<Unit>();
        unit.MaxHealth = baseUnit.maxHealth;
        unit.Health = unit.MaxHealth;
        unit.MovementPoints = baseUnit.movementPoints;
        unit.ManaPoints = baseUnit.manaPoints;
        unit.unitIcon = baseUnit.icon;

        if (baseUnit.enemy)
        {
            Enemy enemy = unitObject.AddComponent<Enemy>();
            enemy.spell = baseUnit.baseSpell;
        }

        BoxCollider2D box = unitObject.AddComponent<BoxCollider2D>();

        AllIn1Shader a1s = unitObject.AddComponent<AllIn1Shader>();
        SetAtlasUvs auv = unitObject.AddComponent<SetAtlasUvs>();

        return unitObject;
    }
}
