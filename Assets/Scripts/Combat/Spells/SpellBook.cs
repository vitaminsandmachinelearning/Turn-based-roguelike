using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public List<GameObject> spells;
    public GameObject currentSpell;
    Unit unit;
    PlayerUI ui;

    public void SelectSpell(int index)
    {
        if (index < spells.Count)
            currentSpell = spells[index];
        else
            currentSpell = null;
    }

    public void CastSpell()
    {
        if (unit == null) unit = GetComponent<Unit>();
        if (ui == null) ui = GetComponent<PlayerUI>();
        if (currentSpell != null && unit.ManaPointsRemaining > 0)
        {
            if (Util.NodesInRange(transform.position, currentSpell.GetComponent<Spell>().castRange).Contains(Util.NearestToCursor()))
            {
                unit.ManaPointsRemaining--;
                var s = Instantiate(currentSpell, (Vector3)Util.NearestToCursor().position, Quaternion.identity);
            }
        }
        ui.UpdateUI();
        currentSpell = null;
    }
}
