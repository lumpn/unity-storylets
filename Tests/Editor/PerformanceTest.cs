using Lumpn.Storylets.Builders;
using NUnit.Framework;
using UnityEngine.Profiling;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class PerformanceTest
    {
        [Test]
        public void ManyRules()
        {
            const int numRules = 1000 * 1000;

            var lookup = new SymbolLookup();
            var ruleset = new RulesetBuilder(lookup);

            var effect = new LogAction("Rule executing");

            Profiler.BeginSample("Ruleset");
            for (int i = 0; i < numRules; i++)
            {
                var rule = ruleset.AddRule(effect);
                rule.AddPredicate(i).EqualTo(10);
            }
            Profiler.EndSample();

            Profiler.BeginSample("Compile");
            var compiledRuleset = ruleset.Build();
            Profiler.EndSample();

            var state = new StateBuilder(lookup);
            var variable = state.AddVariable(numRules / 2);

            variable.Set(0);
            var compiledState1 = state.Build();

            variable.Set(10);
            var compiledState2 = state.Build();

            {
                Profiler.BeginSample("State1");
                var rule = compiledRuleset.Query(compiledState1); // ~150ms
                Profiler.EndSample();
                Assert.IsNull(rule);
            }

            {
                Profiler.BeginSample("State2");
                var rule = compiledRuleset.Query(compiledState2); // ~75ms
                Profiler.EndSample();
                Assert.IsNotNull(rule);
            }
        }
    }
}
