using System.Collections.Generic;
using UnityEngine;

public class WarlockWaypointQuery : BehaviorTree
{
    private float warlockRange;

    public WarlockWaypointQuery(float warlockRange) : base()
    {
        this.warlockRange = warlockRange;
    }

    public override Result Run()
    {
        // check blackboard first
        Transform cachedWaypoint = blackboard?.Get<Transform>("warlockWaypoint");
        if (cachedWaypoint != null) {
            return Result.SUCCESS;
        }

        List<GameObject> enemies = GameManager.Instance.GetEnemiesInRange(Vector3.zero, float.MaxValue);
        AIWaypoint warlockWaypoint = null;
        float minWarlockkDistance = float.MaxValue;

        foreach (var enemy in enemies) {
            if (enemy.GetComponent<EnemyController>().monster == "warlock") {
                Vector3 warlockPos = enemy.transform.position;
                AIWaypoint closestWaypoint = AIWaypointManager.Instance.GetClosestByType(warlockPos, AIWaypoint.Type.SAFE);
                if (closestWaypoint != null) {
                    float distance = Vector3.Distance(warlockPos, closestWaypoint.position);
                    if (distance <= warlockRange && distance < minWarlockkDistance) {
                        minWarlockkDistance = distance;
                        warlockWaypoint = closestWaypoint;
                    }
                }
            }
        }

        if (warlockWaypoint != null) {
            blackboard?.Set("warlockWaypoint", warlockWaypoint.transform);
            return Result.SUCCESS;
        }

        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new WarlockWaypointQuery(warlockRange);
    }
}