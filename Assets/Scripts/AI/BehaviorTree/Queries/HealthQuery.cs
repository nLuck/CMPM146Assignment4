public class HealthQuery : BehaviorTree
{
    private float healthThreshold;

    public HealthQuery(float healthThreshold) : base()
    {
        this.healthThreshold = healthThreshold;
    }

    public override Result Run()
    {
        float currentHealth = agent.hp.hp;
        return currentHealth <= healthThreshold ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new HealthQuery(healthThreshold);
    }
}