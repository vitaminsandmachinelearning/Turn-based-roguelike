using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int MaxHealth;
    public int Health;
    public bool Alive = true;
    public int Initiative = -1;
    public int MovementPointsCap = 5;
    public int MovementPoints = 3;
    public int MovementPointsRemaining;
    public int ManaPointsCap = 5;
    public int ManaPoints = 1;
    public int ManaPointsRemaining;
    public bool processingTurnActions;

    public Sprite unitIcon;
    VisionController vc;
    UnitUI unitUI;

    public void TakeDamage(int damage)
    {
        if (Alive)
        {
            if (GetComponent<StatusEffects>() != null)
            {
                StatusEffects s = GetComponent<StatusEffects>();
                if (s.shockPercentage > 0)
                    damage = (int)(damage * (1f + s.shockPercentage / 100f));
            }
            Health -= damage;
            if (Health <= 0)
                Die();
        }
    }

    public void Die()
    {
        Alive = false;
        FindObjectOfType<TurnOrderUI>().RemoveUnit(this);
        GetComponent<Animator>().SetBool("Dead", true);
        Debug.Log(name);
        SendMessage("OnDie", null, SendMessageOptions.DontRequireReceiver);
    }

    public void ResetStats()
    {
        ManaPointsRemaining = ManaPoints;
        MovementPointsRemaining = MovementPoints;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!name.Equals("Player"))
        {
            //Highlight unit on board
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0.75f);
            GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.yellow);
            FindObjectOfType<TurnController>().hoveredUIUnit = true;

            //Use vision controller to highlight unit movement range
            if (vc == null) vc = FindObjectOfType<VisionController>();
            vc.UnitHovered = gameObject;
            if (unitUI == null) unitUI = FindObjectOfType<UnitUI>();
            unitUI.unit = this;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!name.Equals("Player"))
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0f);
            GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
            FindObjectOfType<TurnController>().hoveredUIUnit = false;

            if (vc == null) vc = FindObjectOfType<VisionController>();
            vc.UnitHovered = null;
            if (unitUI == null) unitUI = FindObjectOfType<UnitUI>();
            unitUI.unit = null;
        }
    }
}
