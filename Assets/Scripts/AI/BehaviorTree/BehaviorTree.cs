using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class BehaviorTree 
{
    public enum Result { SUCCESS, FAILURE, IN_PROGRESS };

    public EnemyController agent;
    protected Blackboard blackboard;

    public virtual Result Run()
    {
        return Result.SUCCESS;
    }

    public BehaviorTree()
    {

    }

    public void SetAgent(EnemyController agent)
    {
        this.agent = agent;
    }

    public void SetBlackboard(Blackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public virtual IEnumerable<BehaviorTree> AllNodes()
    {
        yield return this;
    }

    public virtual BehaviorTree Copy()
    {
        return null;
    }
}
