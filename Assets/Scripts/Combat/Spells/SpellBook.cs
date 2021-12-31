using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public List<BaseSpell> spells;
    public GameObject currentSpell;
    Unit unit;
    PlayerUI ui;

    public bool hasCast = false;

    public void SelectSpell(int index)
    {
        if (index < spells.Count)
        {
            currentSpell = SpellBuilder.Build(spells[index]);
            hasCast = false;
        }
        else
            currentSpell = null;
    }

    public void DeselectSpell()
    {
        if (currentSpell != null)
        {
            if (!hasCast)
                Destroy(currentSpell);
            currentSpell = null;
        }
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
                currentSpell.transform.position = (Vector3)Util.NearestToCursor().position;
                currentSpell.GetComponent<Spell>().Cast();
                hasCast = true;
            }
        }
        ui.UpdateUI();
    }
}
