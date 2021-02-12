﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Storylets.Builders
{
    public sealed class ClusterBuilder
    {
        private readonly List<RuleBuilder> ruleBuilders = new List<RuleBuilder>();
        private readonly SymbolLookup lookup;
        private readonly int minClusterSize;

        public ClusterBuilder(SymbolLookup lookup, int minClusterSize)
        {
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
            return Build(rules);
        }

        private IRuleset Build(IEnumerable<Rule> rules)
        {
            var array = rules.ToArray();
            Array.Sort(array, RuleSpecificityComparer.Default);
            return Build(array);
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
        private IRuleset Build(Rule[] rules)
        {
            var identifiers = rules.SelectMany(p => p.Predicates)
                                   .Select(p => p.identifier)
                                   .Distinct()
                                   .OrderBy(p => p);

            UnityEngine.Debug.LogFormat("Build cluster for {0} rules", rules.Length);

            int bestIdentifier = -1;
            int bestThreshold = 0;
            int numBest = int.MaxValue;
            foreach (var identifier in identifiers)
            {
                var predicates = rules.Where(p => HasPredicate(p, identifier))
                                      .Select(p => GetPredicate(p, identifier))
                                      .ToArray();

                var min = predicates.Min(p => p.min);
                var max = predicates.Max(p => p.max);

                UnityEngine.Debug.LogFormat("id {0}, predicates {1}, min {2}, max {3}", identifier, predicates.Length, min, max);

                var threshold = FindThreshold(predicates, min, max);

                var numBelow = predicates.Count(p => IsBelow(p, threshold));
                var numAbove = predicates.Count(p => IsAbove(p, threshold));
                var numLarger = Math.Max(numBelow, numAbove);

                var numNoPredicate = rules.Length - predicates.Length;
                var num = numLarger + numNoPredicate;

                if (num < numBest)
                {
                    numBest = num;
                    bestIdentifier = identifier;
                    bestThreshold = threshold;
                }

                UnityEngine.Debug.LogFormat("id {0}, threshold {1}, below {2}, above {3}, no predicate {4}, total {5}, best {6}, best id {7}",
                    identifier, threshold, numBelow, numAbove, numNoPredicate, num, numBest, bestIdentifier);
            }

            var numCut = rules.Length - numBest;
            if (numCut < minClusterSize)
            {
                UnityEngine.Debug.LogFormat("Cut {0} too small. Keep {1} rules", numCut, rules.Length);
                return new Ruleset(rules);
            }

            var rulesBelow = rules.Where(p => IsBelow(p, bestIdentifier, bestThreshold)).ToArray();
            var rulesAbove = rules.Where(p => IsAbove(p, bestIdentifier, bestThreshold)).ToArray();

            UnityEngine.Debug.LogFormat("cluster for {0} rules: cut {1}, id {2}, threshold {3}, below {4}, above {5}",
                rules.Length, numCut, bestIdentifier, bestThreshold, rulesBelow.Length, rulesAbove.Length);

            // hierarchical clustering
            var below = Build(rulesBelow);
            var above = Build(rulesAbove);

            return new Cluster(bestIdentifier, bestThreshold, below, above);
        }

        // binary search the threshold that splits the rules into equal halves
        public static int FindThreshold(Predicate[] predicates, int min, int max)
        {
            while (min < max)
            {
                int mid = (int)(((long)min + (long)max) / 2); // int could overflow no matter what
                if (mid == min)
                {
                    return max;
                }

                var numBelow = predicates.Count(p => IsBelow(p, mid));
                var numAbove = predicates.Count(p => IsAbove(p, mid));

                UnityEngine.Debug.LogFormat("min {0}, max {1}, mid {2}, below {3}, above {4}", min, max, mid, numBelow, numAbove);

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

            return max;
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
            var idx = rule.PredicateIndex(identifier);
            if (idx < 0)
            {
                return true;
            }

            var predicate = rule.GetPredicate(idx);
            return IsBelow(predicate, threshold);
        }

        private static bool IsAbove(Rule rule, int identifier, int threshold)
        {
            var idx = rule.PredicateIndex(identifier);
            if (idx < 0)
            {
                return true;
            }

            var predicate = rule.GetPredicate(idx);
            return IsAbove(predicate, threshold);
        }

        private static bool HasPredicate(Rule rule, int identifier)
        {
            var idx = rule.PredicateIndex(identifier);
            return (idx >= 0);
        }

        private static Predicate GetPredicate(Rule rule, int identifier)
        {
            var idx = rule.PredicateIndex(identifier);
            return rule.GetPredicate(idx);
        }
    }
}
