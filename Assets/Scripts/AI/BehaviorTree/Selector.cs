using System.Collections.Generic;

public class Selector : InteriorNode
{
    public override Result Run()
    {
        foreach (var child in children) {
            Result res = child.Run();
            if (res == Result.SUCCESS || res == Result.IN_PROGRESS) {
                return res;
            }
        }
        return Result.FAILURE;
    }

    public Selector(IEnumerable<BehaviorTree> children) : base(children)
    {
    }

    public override BehaviorTree Copy()
    {
        return new Selector(CopyChildren());
    }

}
