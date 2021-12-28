using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
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
    Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void UpdateUI()
    {
        HealthText.text = unit.Health + "/" + unit.MaxHealth;
        Vector3 healthBarScale = new Vector3(unit.Health / unit.MaxHealth, 1, 1);

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
    }
}
