using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Storylets.Builders
{
    public sealed class RulesetBuilder
    {
        private readonly Lookup lookup;
        private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();

        public RulesetBuilder(Lookup lookup)
        {
            this.lookup = lookup;
        }

        public RuleBuilder AddRule(IAction action)
        {
            var builder = new RuleBuilder(lookup, action);
            ruleBuilders.Add(builder);
            return builder;
        }

        public Ruleset Build()
        {
            var rules = ruleBuilders.Select(p => p.Build());
            return new Ruleset(rules);
        }
    }
}
