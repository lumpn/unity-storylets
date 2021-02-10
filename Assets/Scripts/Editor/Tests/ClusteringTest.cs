using System.Linq;
using NUnit.Framework;
using UnityEngine.Profiling;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class ClusteringTest
    {
        [Test]
        public void ClusterAnalysis()
        {
            var lookup = new Lookup();
            var ruleset = new RulesetBuilder(lookup);

            var lowHealth = ruleset.AddRule(new LogAction("Bob could use some water here!"));
            lowHealth.AddPredicate("location").EqualTo("desert");
            lowHealth.AddPredicate("bob/health").LessThan(10);

            var lowMana = ruleset.AddRule(new LogAction("Anybody got some raspberries for bob?"));
            lowMana.AddPredicate("location").EqualTo("forest");
            lowMana.AddPredicate("bob/mana").LessThan(10);

            Profiler.BeginSample("Clustering");
            var clustering = ruleset.BuildClustering();
            Profiler.EndSample();
        }

        [Test]
        public void RandomClustering()
        {
            const int numRules = 100;
            const int numPredicates = 10;
            const int numIdentifiers = 11;
            const int minValue = 0;
            const int maxValue = 100;

            var identifiers = Enumerable.Range(0, numIdentifiers).ToArray();

            var lookup = new Lookup();
            var ruleset = new RulesetBuilder(lookup);
            var effect = new LogAction("effect");
            var random = new System.Random(0);

            for (int i = 0; i < numRules; i++)
            {
                Shuffle(identifiers, random);

                var rule = ruleset.AddRule(effect);
                for (int j = 0; j < numPredicates; j++)
                {
                    var id = identifiers[j];
                    var value = random.Next(minValue, maxValue);

                    var pred = rule.AddPredicate(id);
                    pred.EqualTo(value);
                }
            }

            Profiler.BeginSample("Clustering");
            var clustering = ruleset.BuildClustering();
            Profiler.EndSample();
        }

        private static void Shuffle(int[] values, System.Random random)
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
