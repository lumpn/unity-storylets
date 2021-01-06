using System.Collections.Generic;
using System.Linq;

public sealed class RulesetBuilder
{
    private readonly Lookup lookup;
    private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();

    public RulesetBuilder(Lookup lookup)
    {
        this.lookup = lookup;
    }

    public RuleBuilder AddRule()
    {
        var builder = new RuleBuilder(lookup);
        ruleBuilders.Add(builder);
        return builder;
    }

    public Ruleset Build()
    {
        var rules = ruleBuilders.Select(p => p.Build())
                                .OrderBy(p => p, RuleSpecificityComparer.Default)
                                .ToArray();

        return new Ruleset(rules);
    }
}
