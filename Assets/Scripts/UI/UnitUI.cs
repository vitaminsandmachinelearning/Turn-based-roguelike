using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public Transform HealthBarImage;

    public GameObject ManaCrystal;
    public GameObject MovementCrystal;
    public GameObject EmptyCrystal;

    public Transform ManaCrystalParent;
    public Transform MovementCrystalParent;

    public List<GameObject> ManaCrystalIcons;
    public List<GameObject> MovementCrystalIcons;

    public TextMeshProUGUI FrozenText;
    public TextMeshProUGUI PoisonText;
    public TextMeshProUGUI ShockText;

    public Unit unit;

    void Update()
    {
        if (unit != null)
        {
            if (!unit.name.Equals("Player"))
            {
                HealthText.text = unit.Health + "/" + unit.MaxHealth;
                Vector3 healthBarScale = new Vector3(Mathf.Clamp(unit.Health / (float)unit.MaxHealth, 0f, 1f), 1, 1);
                HealthBarImage.localScale = healthBarScale;

                foreach (GameObject go in ManaCrystalIcons)
                    Destroy(go);
                foreach (GameObject go in MovementCrystalIcons)
                    Destroy(go);
                ManaCrystalIcons.Clear();
                MovementCrystalIcons.Clear();

                for (int i = 0; i < unit.ManaPoints; i++)
                {
                    if (unit.ManaPointsRemaining > i)
                        ManaCrystalIcons.Add(Instantiate(ManaCrystal));
                    else
                        ManaCrystalIcons.Add(Instantiate(EmptyCrystal));
                    ManaCrystalIcons.Last().transform.SetParent(ManaCrystalParent, false);
                }
                for (int i = 0; i < unit.MovementPoints; i++)
                {
                    if (unit.MovementPointsRemaining > i)
                        MovementCrystalIcons.Add(Instantiate(MovementCrystal));
                    else
                        MovementCrystalIcons.Add(Instantiate(EmptyCrystal));
                    MovementCrystalIcons.Last().transform.SetParent(MovementCrystalParent, false);
                }

                FrozenText.text = "Not frozen";
                PoisonText.text = "Not poisoned";
                ShockText.text = "Not shocked";
                if (unit.GetComponent<StatusEffects>() != null)
                {
                    StatusEffects s = unit.GetComponent<StatusEffects>();
                    if (s.freezeDuration > 0)
                        FrozenText.text = s.freezeDuration + " turns";
                    if (s.PoisonStacks().Count() > 0)
                    {
                        int totalDamage = 0;
                        foreach (int[] stack in s.PoisonStacks())
                            totalDamage += stack[1];
                        PoisonText.text = totalDamage + " damage";
                    }
                    if (s.shockPercentage > 0)
                    {
                        ShockText.text = "+" + s.shockPercentage + "% damage";
                    }
                }
            }
            else
            {
                HealthText.text = "";
                Vector3 healthBarScale = new Vector3(0f, 0f, 0f);
                HealthBarImage.localScale = healthBarScale;

                foreach (GameObject go in ManaCrystalIcons)
                    Destroy(go);
                foreach (GameObject go in MovementCrystalIcons)
                    Destroy(go);
                ManaCrystalIcons.Clear();
                MovementCrystalIcons.Clear();
            }
        }
    }
}
