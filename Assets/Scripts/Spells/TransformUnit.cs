using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class TransformUnit : MonoBehaviour
{
    GameObject from;
    public GameObject transformInto;
    public int duration;

    void Awake()
    {
        SendMessage("Register");
    }

    void OnHit()
    {
        from = GetComponent<Spell>().target.gameObject;

        if (from.GetComponent<StatusEffects>() != null)
            if (from.GetComponent<StatusEffects>().transformed != null)
            {
                SendMessage("Finished");
                return;
            }

        var t = Instantiate(transformInto, from.transform.position, Quaternion.identity);
        var s = t.AddComponent<StatusEffects>();
        s.ApplyTransform(from, duration);

        if (from.name.Equals("Player"))
        {
            t.name = "Player";
            from.GetComponent<Unit>().ActiveTurn = false;
        }

        //Insert new unit into turn order
        t.GetComponent<Unit>().Initiative = from.GetComponent<Unit>().Initiative;
        FindObjectOfType<TurnController>().InsertTransformedUnit(t.GetComponent<Unit>(), false);
        FindObjectOfType<TurnOrderUI>().RemoveTurnOrderBox(from.GetComponent<Unit>());
        from.SetActive(false);
        SendMessage("Finished");
    }
}