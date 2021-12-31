using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurnOrderUIUnitHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Unit unit;

    private void Update()
    {
        if(unit != null)
            if (GetComponent<Image>().sprite != unit.unitIcon)
                GetComponent<Image>().sprite = unit.unitIcon;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        unit.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0.75f);
        unit.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.yellow);
        FindObjectOfType<VisionController>().isUnitHoveredUI = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        unit.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0f);
        unit.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
        FindObjectOfType<VisionController>().isUnitHoveredUI = false;
    }
}