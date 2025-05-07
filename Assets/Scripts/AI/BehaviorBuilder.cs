using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BehaviorBuilder
{
    private static bool globalRush = false;

    public static BehaviorTree MakeTree(EnemyController agent)
    {
        Blackboard blackboard = new Blackboard();
        BehaviorTree result = null;
        if (agent.monster == "warlock")
        {
            result = BuildWarlockTree(agent, blackboard);
        }
        else if (agent.monster == "zombie")
        {
            result = BuildZombieTree(agent, blackboard);
        }
        else
        {
            result = BuildSkeletonTree(agent, blackboard);
        }

        // do not change/remove: each node should be given a reference to the agent
        foreach (var n in result.AllNodes())
        {
            n.SetAgent(agent);
            n.SetBlackboard(blackboard);
            blackboard.Set("rush", globalRush);
        }
        return result;
    }

    private static BehaviorTree BuildWarlockTree(EnemyController agent, Blackboard blackboard)
    {
        return new Selector(new BehaviorTree[]
        {
            // heal low health ally
            new Sequence(new BehaviorTree[]
            {
                new FindLowHealthEnemyQuery(30f, 5f),
                new AbilityReadyQuery("heal"),
                new Heal()
            }),
            // perm buff for skeleton
            new Sequence(new BehaviorTree[]
            {
                new AbilityReadyQuery("permabuff"),
                new EnemyTypeQuery("skeleton", 5f),
                new PermaBuff()
            }),
            // temp buff for skeleton
            new Sequence(new BehaviorTree[]
            {
                new AbilityReadyQuery("buff"),
                new EnemyTypeQuery("skeleton", 5f),
                new Buff()
            }),
            // rush if threshold reached
            new Sequence(new BehaviorTree[]
            {
                new ConditionQuery(() => blackboard.Get<bool>("rush")),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            // check enemy count
            new Sequence(new BehaviorTree[]
            {
                new EnemyCountQuery(10),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            // just run
            new FleeFromPlayer(32f, 0.5f)
        });
    }

    private static BehaviorTree BuildZombieTree(EnemyController agent, Blackboard blackboard)
    {
        return new Selector(new BehaviorTree[]
        {
            // rush if threshold reached
            new Sequence(new BehaviorTree[]
            {
                new ConditionQuery(() => blackboard.Get<bool>("rush")),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            // check enemy count
            new Sequence(new BehaviorTree[]
            {
                new EnemyCountQuery(10),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            // try to attack if player gets too close
            new Sequence(new BehaviorTree[]
            {
                new PlayerDistanceQuery(12f),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            new FleeFromPlayer(32f, 0.5f)
        });
    }

    private static BehaviorTree BuildSkeletonTree(EnemyController agent, Blackboard blackboard)
    {
        return new Selector(new BehaviorTree[]
        {
            // try to dodge the projectile
            new Sequence(new BehaviorTree[]
            {
                new ProjectileThreatQuery(6f),
                new Dodge(6f, 0.5f)
            }),
            // rush if threshold reached
            new Sequence(new BehaviorTree[]
            {
                new ConditionQuery(() => blackboard.Get<bool>("rush")),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            // check enemy count
            new Sequence(new BehaviorTree[]
            {
                new EnemyCountQuery(10),
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            }),
            new FleeFromPlayer(32f, 0.5f)
        });
    }
}
