using DG.Tweening;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnBasedMovementAI : VersionedMonoBehaviour
{
    InputController ic;
    Unit unit;
    BlockManager blockManager;
    BlockManager.TraversalProvider traversalProvider;

    public float movementSpeed = 0.25f;

    protected override void Awake()
    {
        base.Awake();

        unit = GetComponent<Unit>();

        blockManager = FindObjectOfType<BlockManager>();
        GetComponent<SingleNodeBlocker>().manager = blockManager;
        GetComponent<SingleNodeBlocker>().BlockAtCurrentPosition();
        
        traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>() { GetComponent<SingleNodeBlocker>() });
    }

    public void Spawn()
    {
        GetComponent<SingleNodeBlocker>().BlockAtCurrentPosition();
    }

    public void StartMovementCalculation(Vector2 targetPosition)
    {
        var p = ABPath.Construct(transform.position, targetPosition, MovementCalculated);
        p.traversalProvider = traversalProvider;
        AstarPath.StartPath(p);
        //p.BlockUntilCalculated();
    }

    void MovementCalculated(Path p)
    {
        unit.MovementPointsRemaining -= p.path.Count - 1;
        SendMessage("StartedMoving", null, SendMessageOptions.DontRequireReceiver);
        StartCoroutine(Move(p));
    }

	IEnumerator Move(Path p)
	{
        for (int i = 0; i < p.vectorPath.Count; i++)
        {
            while (Vector3.Distance(transform.position, p.vectorPath[i]) > 0.01f)
            {
                Debug.Log("Moving " + transform.position + " to " + p.vectorPath[i]);
                //transform.position = Vector2.MoveTowards(transform.position, p.vectorPath[i], Time.deltaTime * movementSpeed);
                transform.position = p.vectorPath[i];
                yield return null;
            }
            transform.position = p.vectorPath[i];
        }
        GetComponent<SingleNodeBlocker>().BlockAtCurrentPosition();
        SendMessage("FinishedMoving");
	}
}