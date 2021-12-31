using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell", order = 1)]
public class BaseSpell : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public RuntimeAnimatorController animatorController;

    [Title("Targetting")]
    public TargetType targetType;
    public int castRange;

    [Title("Area of Effect")]
    public bool AoE;
    public int radius;

    [Title("Damage")]
    public bool Damage;
    public DamageType damageType;
    public int damage;

    [Title("Status Effects")]
    public bool ApplyStatusEffects;
    public int poisonDuration;
    public int poisonDamage;
    public int freezeDuration;
    public int shockPercentage;

    [Title("Propagation")]
    public bool Propagation;
    public int maxPropagations;
    public float propagationDelay;
}