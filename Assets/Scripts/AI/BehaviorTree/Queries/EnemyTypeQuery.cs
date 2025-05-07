using UnityEngine;

public class EnemyTypeQuery : BehaviorTree
{
    private string enemyType;
    private float range;

    public EnemyTypeQuery(string enemyType, float range) : base()
    {
        this.enemyType = enemyType;
        this.range = range;
    }

    public override Result Run()
    {
        var target = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (target != null && Vector3.Distance(agent.transform.position, target.transform.position) <= range) {
            string targetType = target.GetComponent<EnemyController>().monster;
            if (targetType == enemyType) {
                if (blackboard != null) {
                    blackboard.Set("actionTarget", target);
                    return Result.SUCCESS;
                }
            }
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new EnemyTypeQuery(enemyType, range);
    }
}