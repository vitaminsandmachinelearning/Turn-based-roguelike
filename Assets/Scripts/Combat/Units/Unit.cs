using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int MaxHealth = 10;
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
    SpellBook sb;
    VisionController vc;
    UnitUI unitUI;

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        if (GetComponent<TurnBasedMovementAI>() != null) GetComponent<TurnBasedMovementAI>().Spawn();
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        if (Alive)
        {
            if (GetComponent<StatusEffects>() != null)
            {
                StatusEffects s = GetComponent<StatusEffects>();
                switch (damageType)
                {
                    case DamageType.Physical:
                        break;
                    case DamageType.Lightning:
                        if(s.shockPercentage > 0)
                            damage = (int)(damage * (1f + s.shockPercentage / 100f));
                        break;
                    case DamageType.Ice:
                        if (s.freezeDuration > 0)
                            damage *= 2;
                        break;
                    case DamageType.Poison:
                        break;
                }
            }
            Debug.Log(name + " taking " + damage + " " + damageType);
            Health -= damage;
            if (Health <= 0)
                Die();
            if (name.Equals("Player"))
                GetComponent<PlayerUI>().UpdateUI();
        }
    }

    public void Die()
    {
        Alive = false;
        FindObjectOfType<TurnOrderUI>().RemoveUnit(this);
        GetComponent<Animator>().SetBool("Dead", true);
        SendMessage("OnDie", null, SendMessageOptions.DontRequireReceiver);
    }

    public void ResetStats()
    {
        ManaPointsRemaining = ManaPoints;
        MovementPointsRemaining = MovementPoints;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        HighlightUnit();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        UnhighlightUnit();
    }

    void HighlightUnit()
    {
        if (!name.Equals("Player"))
        {
            //Highlight unit on board
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0.75f);
            GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.yellow);
            FindObjectOfType<VisionController>().isUnitHoveredUI = true;

            //Use vision controller to highlight unit movement range
            if (vc == null) vc = FindObjectOfType<VisionController>();

            if (unitUI == null) unitUI = FindObjectOfType<UnitUI>();
            unitUI.UnitPanel.SetActive(true);
            unitUI.unit = this;
        }
    }

    void UnhighlightUnit()
    {
        if (!name.Equals("Player"))
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0f);
            GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
            FindObjectOfType<VisionController>().isUnitHoveredUI = false;

            if (vc.UnitSelected == null)
            {
                if (unitUI == null) unitUI = FindObjectOfType<UnitUI>();
                unitUI.UnitPanel.SetActive(false);
                unitUI.unit = null;
            }
        }
    }
}
