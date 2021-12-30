using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : SpellComponent
{
    ///TODO
    ///Remove shockPercentage from statuseffects and store value as X
    ///for X, apply effect attached to Ground
    public override EffectPriority Getpriority(){ return EffectPriority.PostCast; }
    public override IEnumerator Effect()
    {
        yield return null;
    }
}
