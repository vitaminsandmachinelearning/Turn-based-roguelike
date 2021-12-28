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
    public GameObject transformed;
    public int transformDuration;

    //Indicators held in MapController component on GameController object because script can't find prefabs without shenanigans. Just trust the process.
    GameObject FrozenIndicator;
    GameObject frozenIndicatorInstance;
    GameObject PoisonIndicator;
    List<GameObject> poisonIndicatorInstances = new List<GameObject>();
    GameObject TransformIndicator;
    GameObject transformIndicatorInstance;

    Vector3 statusIndicatorOffset = new Vector3(0f, 0.75f, 0f);

    public void UpdateStatusIndicators()
    {
        //Destroy unnecessary status effect indicators
        if (freezeDuration <= 0 && frozenIndicatorInstance != null)
            Destroy(frozenIndicatorInstance);

        int dif = poisonIndicatorInstances.Count - PoisonStacks().Count;
        for(int i = 0; i < dif; i++)
        {
            foreach (GameObject g in poisonIndicatorInstances)
                Debug.Log(g.name);
            Debug.Log(poisonIndicatorInstances.Any());
            Destroy(poisonIndicatorInstances.Last());
            poisonIndicatorInstances.Remove(poisonIndicatorInstances.Last());
        }

        if (transformDuration <= 0 && transformIndicatorInstance != null)
            Destroy(frozenIndicatorInstance);
    }

    public void AddPoison(int duration, int damage)
    {
        poison.Add(new int[] { duration, damage });
        if (PoisonIndicator == null)
            PoisonIndicator = FindObjectOfType<MapController>().PoisonIndicator;
        poisonIndicatorInstances.Add(Instantiate(PoisonIndicator, transform.position + statusIndicatorOffset, Quaternion.identity));
        poisonIndicatorInstances.Last().transform.SetParent(transform);
    }

    public List<int[]> PoisonStacks()
    {
        poison.RemoveAll(x => x[0] <= 0);
        return poison;
    }

    public void ApplyTransform(GameObject from, int duration)
    {
        transformed = from;
        transformDuration = duration;
        if (TransformIndicator == null)
            TransformIndicator = FindObjectOfType<MapController>().TransformIndicator;
        transformIndicatorInstance = Instantiate(TransformIndicator, transform.position + statusIndicatorOffset, Quaternion.identity);
        transformIndicatorInstance.transform.SetParent(transform);
    }

    public void TransformBack()
    {
        transformed.SetActive(true);
        Destroy(gameObject, GetComponent<Enemy>().turnDelay);
    }

    public void ApplyFreeze(int duration)
    {
        freezeDuration += duration;
        if(FrozenIndicator == null)
            FrozenIndicator = FindObjectOfType<MapController>().FrozenIndicator;
        frozenIndicatorInstance = Instantiate(FrozenIndicator, transform.position + statusIndicatorOffset, Quaternion.identity);
        frozenIndicatorInstance.transform.SetParent(transform);
    }
}
