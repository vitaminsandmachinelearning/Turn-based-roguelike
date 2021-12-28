using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public List<GameObject> spells;
    public GameObject currentSpell;

    public void SelectSpell(int index)
    {
        if (index < spells.Count)
            currentSpell = spells[index];
        else
            currentSpell = null;
    }

    public void CastSpell()
    {
        if (currentSpell != null)
        {
            var s = Instantiate(currentSpell, (Vector3)Util.NearestToCursor().position, Quaternion.identity);
            s.SendMessage("OnCast");
        }
    }
}
