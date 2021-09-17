using System.Collections.Generic;

namespace Lumpn.Storylets
{
    /// orders rules by predicate count descending
    public sealed class RuleSpecificityComparer : IComparer<Rule>
    {
        public static readonly RuleSpecificityComparer Default = new RuleSpecificityComparer();

        public int Compare(Rule a, Rule b)
        {
            return (b.predicateCount - a.predicateCount);
        }
    }
}
