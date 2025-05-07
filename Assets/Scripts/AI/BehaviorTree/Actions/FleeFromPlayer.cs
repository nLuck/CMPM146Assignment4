using UnityEngine;
using System.Collections.Generic;

public class FleeFromPlayer : BehaviorTree
{
    private float minDistance;
    private float arrivedDistance;

    private static readonly List<Vector2> boundaryPoints = new List<Vector2>
    {
        new Vector2(64, 38), new Vector2(64, -26), new Vector2(38, -26), new Vector2(38, -23),
        new Vector2(35, -23), new Vector2(35, -19), new Vector2(32, -19), new Vector2(32, -12),
        new Vector2(30, -12), new Vector2(30, -9), new Vector2(26, -9), new Vector2(26, -7),
        new Vector2(16, -7), new Vector2(16, -10), new Vector2(12, -10), new Vector2(12, -14),
        new Vector2(6, -14), new Vector2(6, -21), new Vector2(-5, -21), new Vector2(-5, -14),
        new Vector2(-14, -14), new Vector2(-14, -7), new Vector2(-23, -7), new Vector2(-23, 1),
        new Vector2(-18, 1), new Vector2(-18, 8), new Vector2(-27, 8), new Vector2(-27, 25),
        new Vector2(2, 25), new Vector2(2, 30), new Vector2(31, 30), new Vector2(31, 32),
        new Vector2(33, 32), new Vector2(33, 34), new Vector2(36, 34), new Vector2(36, 35),
        new Vector2(51, 35), new Vector2(51, 38), new Vector2(64, 38)
    };

    public FleeFromPlayer(float minDistance, float arrivedDistance) : base()
    {
        this.minDistance = minDistance;
        this.arrivedDistance = arrivedDistance;
    }

    public override Result Run()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 enemyPos = agent.transform.position;
        Vector3 direction = (enemyPos - playerPos).normalized;

        float distanceToPlayer = Vector3.Distance(playerPos, enemyPos);
        if (distanceToPlayer >= minDistance) {
            agent.GetComponent<Unit>().movement = Vector2.zero;
            return Result.SUCCESS;
        }

        Vector2 moveDirection = direction;
        Vector3 newPos = enemyPos + (Vector3)(moveDirection.normalized * agent.GetComponent<Unit>().speed * Time.deltaTime);

        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        int n = agent.GetComponent<Rigidbody2D>().Cast(moveDirection, hits, moveDirection.magnitude * Time.deltaTime * 2);

        bool insideMap = IsPointInPolygon(newPos);

        if (n > 0 || !insideMap) {
            Vector2 perp1 = new Vector2(-direction.y, direction.x);
            Vector2 perp2 = new Vector2(direction.y, -direction.x);

            Vector3 testPos1 = enemyPos + (Vector3)(perp1.normalized * agent.GetComponent<Unit>().speed * Time.deltaTime);
            Vector3 testPos2 = enemyPos + (Vector3)(perp2.normalized * agent.GetComponent<Unit>().speed * Time.deltaTime);

            n = agent.GetComponent<Rigidbody2D>().Cast(perp1, hits, perp1.magnitude * Time.deltaTime * 2);
            if (n == 0 && IsPointInPolygon(testPos1)) {
                moveDirection = perp1;
            } else {
                n = agent.GetComponent<Rigidbody2D>().Cast(perp2, hits, perp2.magnitude * Time.deltaTime * 2);
                if (n == 0 && IsPointInPolygon(testPos2)) {
                    moveDirection = perp2;
                } else {
                    agent.GetComponent<Unit>().movement = Vector2.zero;
                    return Result.IN_PROGRESS;
                }
            }
        }

        agent.GetComponent<Unit>().movement = moveDirection.normalized;
        return Result.IN_PROGRESS;
    }

    private bool IsPointInPolygon(Vector3 point)
    {
        int crossings = 0;
        for (int i = 0; i < boundaryPoints.Count - 1; i++) {
            Vector2 p1 = boundaryPoints[i];
            Vector2 p2 = boundaryPoints[i+1];

            if (((p1.y <= point.y && point.y < p2.y) || (p2.y <= point.y && point.y < p1.y)) && point.x < (p2.x - p1.x) * (point.y - p1.y) / (p2.y - p1.y) + p1.x) {
                crossings++;
            }
        }

        return (crossings % 2) == 1;
    }

    public override BehaviorTree Copy()
    {
        return new FleeFromPlayer(minDistance, arrivedDistance);
    }
}