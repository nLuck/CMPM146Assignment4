using UnityEditor;
using UnityEngine;

public class Dodge : BehaviorTree
{
    private float distance;
    private float arrivedDistance;

    public Dodge(float distance, float arrivedDistance) : base()
    {
        this.distance = distance;
        this.arrivedDistance = arrivedDistance;
    }

    public override Result Run()
    {
        GameObject projectile = blackboard?.Get<GameObject>("threatProjectile");
        if (projectile == null) {
            return Result.FAILURE;
        }

        Vector3 projectilePos = projectile.transform.position;
        Vector3 direction = (agent.transform.position - projectilePos).normalized;
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);

        Vector3 targetPos = agent.transform.position + perpendicular * distance;
        Vector3 moveDirection = (targetPos - agent.transform.position);

        if (moveDirection.magnitude <= arrivedDistance) {
            agent.GetComponent<Unit>().movement = Vector2.zero;
            return Result.SUCCESS;
        }

        agent.GetComponent<Unit>().movement = moveDirection.normalized;
        return Result.IN_PROGRESS;
    }

    public override BehaviorTree Copy()
    {
        return new Dodge(distance, arrivedDistance);
    }
}