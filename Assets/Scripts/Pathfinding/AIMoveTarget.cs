using UnityEngine;
using System.Collections;

namespace Pathfinding {
	public class AIMoveTarget : VersionedMonoBehaviour {
        public Transform target;
		IAstarAI ai;
		Unit unit;

        private void Start()
        {
			unit = GetComponent<Unit>();
        }

        void OnEnable () {
			ai = GetComponent<IAstarAI>();
			if (ai != null) ai.onSearchPath += Update;
			AstarPath.active.Scan();
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		void Update () {
			if (target != null && ai != null && unit.ActiveTurn) ai.destination = target.position; 
			if (ai.reachedDestination)
			{
				SendMessage("FinishMoving");
				enabled = false;
			}
		}
	}
}