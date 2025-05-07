using System.Collections.Generic;
using UnityEngine;

public class ProjectileThreatQuery : BehaviorTree
{
    private float range;

    public ProjectileThreatQuery(float range) : base()
    {
        this.range = range;
    }

    public override Result Run()
    {
        var projectiles = Object.FindObjectsByType<ProjectileController>(FindObjectsSortMode.None);
        foreach (var projectile in projectiles) {
            if (Vector3.Distance(projectile.transform.position, agent.transform.position) <= range) {
                blackboard?.Set("threatProjectile", projectile.gameObject);
                return Result.SUCCESS;
            }
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new ProjectileThreatQuery(range);
    }
}