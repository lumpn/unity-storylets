public sealed class Ruleset
{
    /// rules sorted by specificity
    private readonly Rule[] rules;

    public Ruleset(Rule[] rules)
    {
        Assert.NotNull(rules);

        this.rules = rules;
    }

    public Rule Query(State state)
    {
        foreach (var rule in rules)
        {
            if (rule.Matches(state))
            {
                return rule;
            }
        }

        return null;
    }
}
