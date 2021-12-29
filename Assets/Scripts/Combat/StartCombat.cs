using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : MonoBehaviour
{
    Unit[] units;
    TurnController tc;

    public void Initiate()
    {
        units = FindObjectsOfType<Unit>();
        tc = GetComponent<TurnController>();
        tc.StartCombat();
    }
}