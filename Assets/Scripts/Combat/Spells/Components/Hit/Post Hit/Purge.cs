using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purge : SpellComponent
{
    ///TODO
    ///Remove poison stacks from statuseffects and store value as [duration (X), damage (Y)]
    ///for X || Y, apply effect attached to purge.
    public override EffectPriority Getpriority() { return EffectPriority.PostCast; }
    public override IEnumerator Effect()
    {
        yield return null;
    }
}
