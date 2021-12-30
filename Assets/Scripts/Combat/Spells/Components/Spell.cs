using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Delegate to pass component effects to queue
public delegate IEnumerator CallEffect();

//Order of operation for component effects
public enum EffectPriority
{ 
    OnCast,
    PostCast,
    OnHit,
    PostHit
}

public enum TargetType
{ 
    Self, 
    Mouse,
    Position,
    Propagated
}
public class Spell : MonoBehaviour
{
    public GameObject target;
    public TargetType targetType;
    public int castRange;

    SpriteRenderer sprite;
    Animator anim;

    List<(EffectPriority, CallEffect)> EffectQueue;

    void Start()
    {
        switch (targetType)
        {
            case TargetType.Self:
            {
                    target = GameObject.Find("Player");
                    break;
            }
            case TargetType.Mouse:
            { 
                GraphNode nearestToCursor = Util.NearestToCursor();
                foreach (Unit u in FindObjectsOfType<Unit>())
                    if (AstarPath.active.GetNearest(u.transform.position).node.Equals(nearestToCursor))
                        target = u.gameObject;
                break;
            }
            case TargetType.Position:
            {
                GraphNode nearestToPosition = AstarPath.active.GetNearest(transform.position).node;
                foreach (Unit u in FindObjectsOfType<Unit>())
                    if (AstarPath.active.GetNearest(u.transform.position).node == nearestToPosition)
                    {
                        target = u.gameObject;
                    }
                break;
            }
        }
        StartCoroutine(ProcessEffectQueue());
    }

    IEnumerator ProcessEffectQueue()
    {
        if (EffectQueue != null && target != null)
        {
            EffectQueue = EffectQueue.OrderBy(x => x.Item1).ToList();
            foreach ((EffectPriority, CallEffect) e in EffectQueue)
            {
                yield return StartCoroutine(e.Item2());
            }
        }
        while (!GetComponent<Animator>().GetBool("Finished"))
            yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    void Register((EffectPriority, CallEffect) e)
    {
        if (EffectQueue == null)
            EffectQueue = new List<(EffectPriority, CallEffect)>();
        EffectQueue.Add(e);
    }
}