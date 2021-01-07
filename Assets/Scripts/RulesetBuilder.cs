using System.Collections.Generic;
using System.Linq;

public sealed class RulesetBuilder
{
    private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();

    public RuleBuilder AddRule(IEffect effect)
    {
        var builder = new RuleBuilder(effect);
        ruleBuilders.Add(builder);
        return builder;
    }

    public Ruleset Build()
    {
        var rules = BuildRules();
        return new Ruleset(rules);
    }

    public IRuleset BuildClustering()
    {
        var rules = BuildRules();
        var clusterBuilder = new ClusterBuilder(rules);
        return clusterBuilder.Build();
    }

    private Rule[] BuildRules()
    {
        return ruleBuilders.Select(p => p.Build())
                           .OrderBy(p => p, RuleSpecificityComparer.Default)
                           .ToArray();
    }
}
