using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdateCurrentSpell : MonoBehaviour
{
    SpellBook sb;
    private void Start()
    {
        sb = FindObjectOfType<SpellBook>();
    }
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = sb.currentSpell != null ? sb.currentSpell.name : "No Spell Equipped";
    }
}
