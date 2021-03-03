using System;
using System.Collections.Generic;
using System.Linq;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Builders
{
    public sealed class ClusterBuilder
    {
        private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();
        private readonly SymbolLookup lookup;
        private readonly int minClusterSize;

        public ClusterBuilder(SymbolLookup lookup, int minClusterSize)
        {
            DebugAssert.NotNull(lookup);
            DebugAssert.Greater(minClusterSize, 0);

            this.lookup = lookup;
            this.minClusterSize = minClusterSize;
        }

        public RuleBuilder AddRule(IAction action)
        {
            var builder = new RuleBuilder(lookup, action);
            ruleBuilders.Add(builder);
            return builder;
        }

        public IRuleset Build()
        {
            var rules = ruleBuilders.Select(p => p.Build());
            return Build(rules, minClusterSize);
        }

        public static IRuleset Build(IEnumerable<Rule> rules, int minClusterSize)
        {
            DebugAssert.NotNull(rules);
            DebugAssert.Greater(minClusterSize, 0);

            var array = rules.ToArray();
            Array.Sort(array, RuleSpecificityComparer.Default);
            return Build(array, minClusterSize);
        }

        //                     T                      threshold
        // |----------------------------------------| range
        //            |-----|                         rule 1 (min <= max < t)
        //               |-----|                      rule 2 (min <= max = t)
        //                  |-----|                   rule 3 (min <= t <= max)
        //                     |-----|                rule 4 (t = min <= max)
        //                        |-----|             rule 5 (t < min <= max)
        //
        // |------------------|                       below range
        //                     |--------------------| above range
        // => below: rule 1-3
        // => above: rule 2-5
        private static IRuleset Build(Rule[] rules, int minClusterSize)
        {
            var identifiers = rules.SelectMany(p => p.Predicates)
                                   .Select(p => p.identifier)
                                   .Distinct()
                                   .OrderBy(p => p)
                                   .ToArray();

            // find identifier and threshold which cuts away the most rules
            int bestIdentifier = -1;
            int bestThreshold = 0;
            int bestSize = int.MaxValue;
            foreach (var identifier in identifiers)
            {
                // relevant predicates
                var predicates = rules.Where(p => HasPredicate(p, identifier))
                                      .Select(p => GetPredicate(p, identifier))
                                      .ToArray();

                // find threshold
                var min = predicates.Min(p => p.min);
                var max = predicates.Max(p => p.max);
                var threshold = FindThreshold(predicates, min, max);

                // count halves
                var numBelow = predicates.Count(p => IsBelow(p, threshold));
                var numAbove = predicates.Count(p => IsAbove(p, threshold));
                var numLarger = Math.Max(numBelow, numAbove);

                // rules without this identifier go in both halves
                var numNoPredicate = rules.Length - predicates.Length;
                var halfSize = numLarger + numNoPredicate;

                if (halfSize < bestSize)
                {
                    bestSize = halfSize;
                    bestIdentifier = identifier;
                    bestThreshold = threshold;
                }
            }

            if (bestIdentifier < 0)
            {
                // no identifier found to cut
                return new Ruleset(rules);
            }

            var numCut = rules.Length - bestSize;
            if (numCut < minClusterSize)
            {
                // cut too small
                return new Ruleset(rules);
            }

            // split rules
            var rulesBelow = rules.Where(p => IsBelow(p, bestIdentifier, bestThreshold)).ToArray();
            var rulesAbove = rules.Where(p => IsAbove(p, bestIdentifier, bestThreshold)).ToArray();

            // hierarchical clustering
            var below = Build(rulesBelow, minClusterSize);
            var above = Build(rulesAbove, minClusterSize);
            return new Cluster(bestIdentifier, bestThreshold, below, above);
        }

        // binary search the threshold that splits the rules into equal halves
        // using long because int arithmetic could overflow no matter what
        public static int FindThreshold(Predicate[] predicates, long min, long max)
        {
            DebugAssert.NotNull(predicates);
            DebugAssert.LessOrEqual(min, max);

            while (min < max - 1)
            {
                int mid = (int)((min + max) / 2);
                var numBelow = predicates.Count(p => IsBelow(p, mid));
                var numAbove = predicates.Count(p => IsAbove(p, mid));

                if (numBelow == numAbove)
                {
                    return mid;
                }
                if (numBelow < numAbove)
                {
                    min = mid;
                }
                else
                {
                    max = mid;
                }
            }

            return (int)max;
        }

        private static bool IsBelow(Predicate predicate, int threshold)
        {
            return (predicate.min < threshold);
        }

        private static bool IsAbove(Predicate predicate, int threshold)
        {
            return (predicate.max >= threshold);
        }

        private static bool IsBelow(Rule rule, int identifier, int threshold)
        {
            if (!rule.TryGetPredicate(identifier, out Predicate predicate))
            {
                return true;
            }
            return IsBelow(predicate, threshold);
        }

        private static bool IsAbove(Rule rule, int identifier, int threshold)
        {
            if (!rule.TryGetPredicate(identifier, out Predicate predicate))
            {
                return true;
            }
            return IsAbove(predicate, threshold);
        }

        private static bool HasPredicate(Rule rule, int identifier)
        {
            return rule.TryGetPredicate(identifier, out Predicate predicate);
        }

        private static Predicate GetPredicate(Rule rule, int identifier)
        {
            rule.TryGetPredicate(identifier, out Predicate predicate);
            return predicate;
        }
    }
}
