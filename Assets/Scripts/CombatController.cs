using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public List<BaseUnit> PossibleUnits;
    public List<Vector3> spawnPoints;

    Unit[] units;
    TurnController tc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Initiate();
    }

    void Initiate()
    {
        SpawnUnits();
    }

    void SpawnUnits()
    {
        foreach (Vector3 pos in spawnPoints)
        {
            var u = UnitBuilder.Build(PossibleUnits[Random.Range(0, PossibleUnits.Count)]);
            u.GetComponent<Unit>().Spawn(pos);
        }
        Run();
    }

    void Run()
    {
        units = FindObjectsOfType<Unit>();
        tc = GetComponent<TurnController>();
        tc.StartCombat();
    }
}
