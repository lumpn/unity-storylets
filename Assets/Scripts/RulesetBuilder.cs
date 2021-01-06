using System.Collections.Generic;
using System.Linq;

public sealed class RulesetBuilder
{
    public readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();

    public Ruleset Build(Lookup lookup)
    {
        var rules = ruleBuilders.Select(p => p.Build(lookup))
                                .OrderBy(p => p, RuleSpecificityComparer.Default)
                                .ToArray();

        return new Ruleset(rules);
    }
}
