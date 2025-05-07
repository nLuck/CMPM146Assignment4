using UnityEngine;

public class PermaBuff : BehaviorTree
{
    public override Result Run()
    {
        GameObject target = blackboard?.Get<GameObject>("actionTarget");
        if (target == null) {
            target = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        }
        EnemyAction act = agent.GetAction("permabuff");
        if (act == null || target == null) return Result.FAILURE;

        bool success = act.Do(target.transform);
        return (success ? Result.SUCCESS : Result.FAILURE);
    }

    public PermaBuff() : base()
    {

    }

    public override BehaviorTree Copy()
    {
        return new PermaBuff();
    }
}
