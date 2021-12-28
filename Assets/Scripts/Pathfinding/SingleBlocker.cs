using UnityEngine;
using System.Collections;
using Pathfinding;

public class SingleBlocker : MonoBehaviour
{
    public void Start()
    {
        var blocker = GetComponent<SingleNodeBlocker>();
        blocker.manager = FindObjectOfType<BlockManager>();
        blocker.BlockAtCurrentPosition();
        Debug.Log("block");
    }
}