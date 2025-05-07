using UnityEngine;

public class FindLowHealthEnemyQuery : BehaviorTree
{
    private float healthThreshold;
    private float range;

    public FindLowHealthEnemyQuery(float healthThreshold, float range) : base()
    {
        this.healthThreshold = healthThreshold;
        this.range = range;
    }

    public override Result Run()
    {
        var enemies = GameManager.Instance.GetEnemiesInRange(agent.transform.position, range);
        GameObject lowHealthEnemy = null;
        float minHealth = float.MaxValue;

        foreach (var enemy in enemies) {
            if (enemy != agent.gameObject) {
                float health = enemy.GetComponent<EnemyController>().hp.hp;
                if (health <= healthThreshold && health < minHealth && enemy.GetComponent<EnemyController>().GetEffect("noheal") == 0) {
                    minHealth = health;
                    lowHealthEnemy = enemy;
                }
            }
        }

        if (lowHealthEnemy != null) {
            blackboard?.Set("actionTarget", lowHealthEnemy);
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new FindLowHealthEnemyQuery(healthThreshold, range);
    }
}