using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Storylets
{
    public sealed class RulesetBuilder
    {
        private readonly Lookup lookup;
        private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();

        public RulesetBuilder(Lookup lookup)
        {
            this.lookup = lookup;
        }

        public RuleBuilder AddRule(IEffect effect)
        {
            var builder = new RuleBuilder(lookup, effect);
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
            return ClusterBuilder.Build(rules);
        }

        private Rule[] BuildRules()
        {
            return ruleBuilders.Select(p => p.Build())
                               .OrderBy(p => p, RuleSpecificityComparer.Default)
                               .ToArray();
        }
    }
}
