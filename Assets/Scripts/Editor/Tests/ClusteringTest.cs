using NUnit.Framework;
using UnityEngine.Profiling;

namespace Test
{
    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public sealed class ClusteringTest
    {
        [Test]
        public void ClusterAnalysis()
        {
            var lookup = new Lookup();
            var ruleset = new RulesetBuilder(lookup);

            var lowHealth = ruleset.AddRule(new LogEffect("Bob could use some water here!"));
            lowHealth.AddPredicate("location").EqualTo("desert");
            lowHealth.AddPredicate("bob/health").LessThan(10);

            var lowMana = ruleset.AddRule(new LogEffect("Anybody got some raspberries for bob?"));
            lowMana.AddPredicate("location").EqualTo("forest");
            lowMana.AddPredicate("bob/mana").LessThan(10);

            Profiler.BeginSample("Clustering");
            var clustering = ruleset.BuildClustering();
            Profiler.EndSample();
        }
    }
}
