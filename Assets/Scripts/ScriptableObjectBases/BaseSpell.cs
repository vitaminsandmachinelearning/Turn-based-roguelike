using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseSpell : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public RuntimeAnimatorController animationController;

    [Title("Targetting")]
    public TargetType targetType;
    public int CastRange;

    [Title("Area of Effect")]
    public bool AoE;
    public int radius;

    [Title("Damage")]
    public bool Damage;
    public DamageType damageType;
    public int damage;

    [Title("Status Effects")]
    public bool ApplyStatusEffects;
    public int PoisonStackDuration;
    public int PoisonStackDamage;
    public int FreezeDuration;
    public int ShockPercentage;

    [Title("Propagation")]
    public bool Propagation;
    public int MaxBounces;
    public float DelayBetweenBounces;


}

