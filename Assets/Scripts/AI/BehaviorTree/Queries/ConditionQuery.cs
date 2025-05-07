using System;

public class ConditionQuery : BehaviorTree
{
    private Func<bool> condition;

    public ConditionQuery(Func<bool> condition) : base()
    {
        this.condition = condition;
    }

    public override Result Run()
    {
        return condition() ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new ConditionQuery(condition);
    }
}