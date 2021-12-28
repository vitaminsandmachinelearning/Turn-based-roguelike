using Assets.Scripts;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int registered = 0;
    int finished = 0;

    bool hasHit = false;

    void OnCast()
    {
        //USE "OnPostCast" INSTEAD OF SENDING "OnCast" FROM "OnCast"
        SendMessage("OnPostCast", null, SendMessageOptions.DontRequireReceiver);

        switch (targetType)
        {
            case TargetType.Self:
                target = GameObject.Find("Player");
                SendMessage("OnPostCast");
                break;
            case TargetType.Mouse:
                GraphNode nearestToCursor = Util.NearestToCursor();
                foreach (Unit u in FindObjectsOfType<Unit>())
                {
                    if (AstarPath.active.GetNearest(u.transform.position).node.Equals(nearestToCursor))
                    {
                        target = u.gameObject;
                        return;
                    }
                }
                break;
            case TargetType.Position:
                {
                    GraphNode nearestToPosition = AstarPath.active.GetNearest(transform.position).node;
                    foreach (Unit u in FindObjectsOfType<Unit>())
                    {
                        if (AstarPath.active.GetNearest(u.transform.position).node == nearestToPosition)
                        {
                            target = u.gameObject;
                            return;
                        }
                    }
                    break;
                }
            case TargetType.Propagated:
                {
                    GraphNode nearestToPosition = AstarPath.active.GetNearest(transform.position).node;
                    foreach (Unit u in FindObjectsOfType<Unit>())
                    {
                        if (AstarPath.active.GetNearest(u.transform.position).node == nearestToPosition)
                        {
                            target = u.gameObject;
                            return;
                        }
                    }
                    break;
                }
        }
        if (target == null)
        {
            finished = int.MaxValue;
        }
    }

    void Register()
    {
        registered++;
    }

    void Finished()
    {
        finished++;
    }

    private void Update()
    {
        // PROJECTILE CODE
        GraphNode nearestNode = AstarPath.active.GetNearest(transform.position).node;
        if (!nearestNode.Walkable)
            Destroy(gameObject);
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 0.01f && !hasHit)
            {
                hasHit = true;
                SendMessage("OnHit");
            }
        }
        if (registered <= finished && GetComponent<Animator>().GetBool("Finished"))
        {
            Destroy(gameObject);
        }
    }
}