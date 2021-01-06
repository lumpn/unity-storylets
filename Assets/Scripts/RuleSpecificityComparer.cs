using System.Collections.Generic;

public sealed class RuleSpecificityComparer : IComparer<Rule>
{
    public static readonly RuleSpecificityComparer Default = new RuleSpecificityComparer();

    public int Compare(Rule a, Rule b)
    {
        return a.predicateCount - b.predicateCount; // TODO Jonas: or the other way around?
    }
}
