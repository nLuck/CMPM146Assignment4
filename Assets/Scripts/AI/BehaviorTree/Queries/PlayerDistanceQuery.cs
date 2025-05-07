using UnityEngine;

public class PlayerDistanceQuery : BehaviorTree
{
    private float distanceThreshold;

    public PlayerDistanceQuery(float distanceThreshold) : base()
    {
        this.distanceThreshold = distanceThreshold;
    }

    public override Result Run()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 enemyPos = agent.transform.position;
        float distance = Vector3.Distance(playerPos, enemyPos);
        return distance <= distanceThreshold ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new PlayerDistanceQuery(distanceThreshold);
    }
}