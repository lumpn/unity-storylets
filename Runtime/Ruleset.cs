using System;
using System.Collections.Generic;
using System.Linq;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets
{
    public sealed class Ruleset : IRuleset
    {
        /// rules sorted by specificity
        private readonly Rule[] rules;

        public Ruleset(IEnumerable<Rule> rules)
        {
            DebugAssert.NotNull(rules);

            var array = rules.ToArray();
            Array.Sort(array, RuleSpecificityComparer.Default);
            this.rules = array;
        }

        public Rule Query(IState state)
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
}
