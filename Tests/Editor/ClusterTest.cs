using System;
using System.Linq;
using Lumpn.Storylets.Builders;
using NUnit.Framework;
using UnityEngine.Profiling;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class ClusterTest
    {
        [Test]
        public void TestFindThreshold()
        {
            //   0 1 2 3 4 5 6 7 8 9
            // a |                   a
            // b |-----|             b
            // c         |-----|     c
            // d             |       d
            // e           |-------| e
            // f     |               f
            // g     |-------|
            var a = new Predicate(1, 0, 0);
            var b = new Predicate(1, 0, 3);
            var c = new Predicate(1, 4, 7);
            var d = new Predicate(1, 6, 6);
            var e = new Predicate(1, 5, 9);
            var f = new Predicate(1, 2, 2);
            var predicates = new[] { a, b, c, d, e, f };

            Assert.AreEqual(4, ClusterBuilder.FindThreshold(predicates, -100, 100));
            Assert.AreEqual(4, ClusterBuilder.FindThreshold(predicates, int.MinValue, int.MaxValue));
            Assert.AreEqual(4, ClusterBuilder.FindThreshold(predicates, 0, 9));
            Assert.AreEqual(4, ClusterBuilder.FindThreshold(predicates, 0, 10));

            var g = new Predicate(1, 2, 6);
            var predicates2 = new[] { a, b, c, d, e, f, g };

            Assert.AreEqual(4, ClusterBuilder.FindThreshold(predicates2, -100, 100));

            var action = new LogAction("action");
            var rules = predicates.Select(p => new Rule(new[] { p }, action));
            var ruleset = ClusterBuilder.Build(rules, 1);
            Assert.IsTrue(ruleset is Cluster);  // 6 rules

            var cluster = (Cluster)ruleset;
            Assert.AreEqual(1, cluster.identifier);
            Assert.AreEqual(4, cluster.threshold);
            Assert.IsTrue(cluster.below is Cluster); // 3 rules
            Assert.IsTrue(cluster.above is Ruleset); // 3 rules

            var below = (Cluster)cluster.below;
            Assert.AreEqual(1, below.identifier);
            Assert.AreEqual(1, below.threshold);
            Assert.IsTrue(below.below is Ruleset); // 2 rules
            Assert.IsTrue(below.above is Ruleset); // 2 rules
        }

        [Test]
        public void TestFindThresholdShuffled()
        {
            var random = new Random(0);
            var values = Enumerable.Range(0, 1000).ToArray();
            Shuffle(values, random);

            // no overlap
            var predicates = values.Select(p => new Predicate(1, p, p)).ToArray();
            Assert.AreEqual(500, ClusterBuilder.FindThreshold(predicates, int.MinValue, int.MaxValue));

            // some overlap
            var predicates2 = values.Select(p => new Predicate(1, p - 5, p + 5)).ToArray();
            Assert.AreEqual(500, ClusterBuilder.FindThreshold(predicates2, int.MinValue, int.MaxValue));

            var action = new LogAction("action");
            var rules = predicates.Select(p => new Rule(new[] { p }, action));
            var ruleset = ClusterBuilder.Build(rules, 100);
            Assert.IsTrue(ruleset is Cluster);  // 1000 rules

            var cluster = (Cluster)ruleset;
            Assert.AreEqual(1, cluster.identifier);
            Assert.AreEqual(500, cluster.threshold);
        }

        [Test]
        public void TestRandomClustering()
        {
            const int numRules = 100;
            const int numPredicates = 10;
            const int numIdentifiers = 11;
            const int minValue = 0;
            const int maxValue = 100;

            var identifiers = Enumerable.Range(0, numIdentifiers).ToArray();

            var lookup = new SymbolLookup();
            var ruleset = new ClusterBuilder(lookup, 1);
            var action = new LogAction("action");
            var random = new Random(0);

            for (int i = 0; i < numRules; i++)
            {
                Shuffle(identifiers, random);

                var rule = ruleset.AddRule(action);
                for (int j = 0; j < numPredicates; j++)
                {
                    var id = identifiers[j];
                    var value = random.Next(minValue, maxValue);

                    var pred = rule.AddPredicate(id);
                    pred.EqualTo(value);
                }
            }

            Profiler.BeginSample("Clustering");
            var clustering = ruleset.Build();
            Profiler.EndSample();
        }

        /// Fisher-Yates shuffle
        private static void Shuffle(int[] values, Random random)
        {
            int n = values.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int j = random.Next(i, n);
                var tmp = values[i];
                values[i] = values[j];
                values[j] = tmp;
            }
        }
    }
}
