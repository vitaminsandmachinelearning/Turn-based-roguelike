using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : SpellComponent
{
    ///TODO
    ///Remove frozenduration from statuseffects and store value as X
    ///for X, apply effect attached to shatter.
    public override EffectPriority Getpriority() { return EffectPriority.PostCast; }
    public override IEnumerator Effect()
    {
        yield return null;
    }
}