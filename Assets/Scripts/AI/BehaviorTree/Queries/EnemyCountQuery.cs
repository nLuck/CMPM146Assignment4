using System.Collections.Generic;
using UnityEngine;

public class EnemyCountQuery : BehaviorTree
{
    private int countThreshold;

    public EnemyCountQuery(int countThreshold) : base()
    {
        this.countThreshold = countThreshold;
    }

    public override Result Run()
    {
        int count = 0;
        List<GameObject> enemies = GameManager.Instance.GetEnemiesInRange(Vector3.zero, float.MaxValue);
        foreach (var enemy in enemies) {
            string monsterType = enemy.GetComponent<EnemyController>().monster;
            if (monsterType == "zombie" || monsterType == "skeleton") {
                count++;
            }
        }

        if (count >= countThreshold) {
            blackboard?.Set("rush", true);
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new EnemyCountQuery(countThreshold);
    }
}