using System.Linq;
using System;

public static class ClusterBuilder
{
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
    public static IRuleset Build(Rule[] rules)
    {
        var identifiers = rules.SelectMany(p => p.Predicates)
                               .Select(p => p.identifier)
                               .Distinct()
                               .OrderBy(p => p);

        int bestIdentifier = -1;
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
            }

            UnityEngine.Debug.LogFormat("id {0}, threshold {1}, below {2}, above {3}, no predicate {4}, total {5}, best {6}, best id {7}",
                identifier, threshold, numBelow, numAbove, numNoPredicate, num, numBest, bestIdentifier);
        }

        // TODO Jonas: split the rules and create cluster

        return null;
    }

    // binary search the threshold that splits the rules into equal halves
    private static int FindThreshold(Predicate[] predicates, int min, int max)
    {
        while (min < max)
        {
            int mid = (int)(((long)max + (long)min) / 2); // int could overflow no matter what
            if (mid == min)
            {
                return max;
            }

            var numBelow = predicates.Count(p => IsBelow(p, mid));
            var numAbove = predicates.Count(p => IsAbove(p, mid));

            UnityEngine.Debug.LogFormat("min {0}, mid {1}, max {2}, below {3}, above {4}", min, mid, max, numBelow, numAbove);

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
        return predicate.min < threshold;
    }

    private static bool IsAbove(Predicate predicate, int threshold)
    {
        return predicate.max >= threshold;
    }

    private static bool HasPredicate(Rule rule, int identifier)
    {
        var idx = PredicateIndex(rule, identifier);
        return idx >= 0;
    }

    private static Predicate GetPredicate(Rule rule, int identifier)
    {
        var idx = PredicateIndex(rule, identifier);
        return rule.Predicates[idx];
    }

    private static int PredicateIndex(Rule rule, int identifier)
    {
        var predicates = rule.Predicates;
        var key = new Predicate(identifier, 0, 0);

        return Array.BinarySearch(predicates, key, PredicateIdentifierComparer.Default);
    }
}
