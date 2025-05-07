using System.Collections.Generic;

public class Inverter : BehaviorTree
{
    private BehaviorTree child;

    public Inverter(BehaviorTree child) : base()
    {
        this.child = child;
    }

    public override Result Run()
    {
        Result res = child.Run();
        if (res == Result.SUCCESS) {
            return Result.FAILURE;
        } else if (res == Result.FAILURE) {
            return Result.SUCCESS;
        }
        return res;
    }

    public override BehaviorTree Copy()
    {
        return new Inverter(child.Copy());
    }

    public override IEnumerable<BehaviorTree> AllNodes()
    {
        yield return this;
        foreach (var n in child.AllNodes()) {
            yield return n;
        }
    }
}