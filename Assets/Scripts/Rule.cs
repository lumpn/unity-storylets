public sealed class Rule
{
    /// predicates sorted by identifier
    private readonly Predicate[] condition;
    private readonly IEffect effect;

    public int predicateCount { get { return condition.Length; } }

    public Rule(Predicate[] condition, IEffect effect)
    {
        Assert.NotNull(condition);
        Assert.NotNull(effect);

        this.condition = condition;
        this.effect = effect;
    }

    public bool Matches(State state)
    {
        foreach (var predicate in condition)
        {
            if (!predicate.Matches(state))
            {
                return false;
            }
        }
        return true;
    }

    public void Execute()
    {
        effect.Apply();
    }
}
