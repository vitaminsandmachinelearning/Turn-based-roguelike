using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    StartCombat sc;

    private void Start()
    {
        sc = GetComponent<StartCombat>();
        sc.Initiate();
    }
}
