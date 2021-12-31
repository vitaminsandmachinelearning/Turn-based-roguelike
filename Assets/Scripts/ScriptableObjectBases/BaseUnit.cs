using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Unit", order = 2)]
public class BaseUnit : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public RuntimeAnimatorController animatorController;
    public Material material;

    [Title("Stats")]
    public int maxHealth;
    public int movementPoints;
    public int manaPoints;

    [Title("Enemy")]
    public bool enemy;
    public BaseSpell baseSpell;

    [Title("Movement")]
    public int movementSpeed;
}