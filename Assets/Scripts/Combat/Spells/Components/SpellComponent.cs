using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellComponent : MonoBehaviour
{
    public abstract EffectPriority Getpriority();

    protected CallEffect callEffect;

    public void Awake()
    {
        callEffect = Effect;
        SendMessage("Register", (Getpriority(), callEffect));
    }

    public abstract IEnumerator Effect();
}
