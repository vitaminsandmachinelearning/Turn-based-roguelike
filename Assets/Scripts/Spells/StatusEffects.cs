using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    // Duration measured in turns
    // DoT array format: [0]duration [1]damage
    [ShowInInspector]
    private List<int[]> poison = new List<int[]>();
    public int freezeDuration;
    public int shockPercentage;

    GameObject FrozenIndicatorInstance;
    GameObject PoisonIndicatorInstance;
    GameObject ShockIndicatorInstance;
    StatusEffectData sed;

    Vector3 statusIndicatorOffset = new Vector3(0f, 0.75f, 0f);
    
    public void UpdateStatusIndicators()
    {
        CheckReferences();
        Debug.Log("Freeze: " + freezeDuration + " Instance: " + (FrozenIndicatorInstance != null) + "\n" +
            "Shock: " + shockPercentage + " Instance: " + (ShockIndicatorInstance != null) + "\n" +
            "Poison: " + PoisonStacks().Count() + " Instance: " + (PoisonIndicatorInstance != null) + "\n");

        if (freezeDuration <= 0 && FrozenIndicatorInstance != null)
            Destroy(FrozenIndicatorInstance);
        else if (freezeDuration > 0 && FrozenIndicatorInstance != null)
        {
            FrozenIndicatorInstance.GetComponent<Animator>().SetInteger("Value", freezeDuration);
        }
        else if (freezeDuration > 0)
        {
            FrozenIndicatorInstance = Instantiate(sed.FrozenIndicator, transform.position + statusIndicatorOffset, Quaternion.identity);
            FrozenIndicatorInstance.transform.SetParent(transform);
        }

        if ((shockPercentage <= 0) && ShockIndicatorInstance != null)
            Destroy(ShockIndicatorInstance);
        else if (shockPercentage > 0 && ShockIndicatorInstance != null)
        {
            ShockIndicatorInstance.GetComponent<Animator>().SetInteger("Value", shockPercentage);
        }
        else if (shockPercentage > 0)
        {
            ShockIndicatorInstance = Instantiate(sed.ShockIndicator, transform.position + statusIndicatorOffset, Quaternion.identity);
            ShockIndicatorInstance.transform.SetParent(transform);
        }

        if (PoisonStacks().Count() <= 0 && PoisonIndicatorInstance != null)
            Destroy(PoisonIndicatorInstance);
        else if (PoisonStacks().Count() > 0 && PoisonIndicatorInstance != null)
        {
            PoisonIndicatorInstance.GetComponent<Animator>().SetInteger("Value", PoisonStacks().Count());
        }
        else if (PoisonStacks().Count() > 0)
        {
            PoisonIndicatorInstance = Instantiate(sed.PoisonIndicator, transform.position + statusIndicatorOffset, Quaternion.identity);
            PoisonIndicatorInstance.transform.SetParent(transform);
        }

        UpdateStatusIndicatorPositions();
    }
    
    void UpdateStatusIndicatorPositions()
    {
        List<GameObject> allIndicators = new List<GameObject>();
        if (PoisonIndicatorInstance != null) allIndicators.Add(PoisonIndicatorInstance);
        if (FrozenIndicatorInstance != null) allIndicators.Add(FrozenIndicatorInstance);
        if (ShockIndicatorInstance != null) allIndicators.Add(ShockIndicatorInstance);

        float indicatorStartPositionX = -0.4f;
        float spacePerIndicator = Mathf.Abs(indicatorStartPositionX) * 2f / allIndicators.Count();
        Vector3 currentIndicatorPosition = new Vector3(indicatorStartPositionX, 0, 0);

        for(int i = 0; i < allIndicators.Count; i++)
        {
            allIndicators[i].transform.position = transform.position + statusIndicatorOffset + currentIndicatorPosition;
            currentIndicatorPosition.x += spacePerIndicator;
        }
    }

    public void AddPoison(int duration, int damage)
    {
        poison.Add(new int[] { duration, damage });
        UpdateStatusIndicators();
    }

    public List<int[]> PoisonStacks()
    {
        poison.RemoveAll(x => x[0] <= 0);
        return poison;
    }

    public void ApplyFreeze(int duration)
    {
        freezeDuration += duration;
        UpdateStatusIndicators();
    }

    public void ApplyShock(int percentage)
    {
        shockPercentage += percentage;
        UpdateStatusIndicators();
    }

    void CheckReferences()
    {
        sed = FindObjectOfType<StatusEffectData>();
    }
}
