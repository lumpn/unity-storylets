using NUnit.Framework;

namespace Test
{
    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public sealed class Demo
    {
        [Test]
        public void Test()
        {
            var lookup = new Lookup();
            var ruleset = new RulesetBuilder(lookup);

            var lowHealth = ruleset.AddRule();
            lowHealth.AddPredicate("location").EqualTo("desert");
            lowHealth.AddPredicate("bob/health").LessThan(10);
            lowHealth.effect = new LogEffect("Bob could use some water here!");

            var lowMana = ruleset.AddRule();
            lowMana.AddPredicate("location").EqualTo("forest");
            lowMana.AddPredicate("bob/mana").LessThan(10);
            lowMana.effect = new LogEffect("Anybody got some raspberries for bob?");

            var state = new StateBuilder(lookup);
            var location = state.AddVariable("location").Set("forest");
            var health = state.AddVariable("bob/health").Set(100);
            var mana = state.AddVariable("bob/mana").Set(100);

            {
                var rule = Query(ruleset, state);
                Assert.IsNull(rule); // no match
            }

            {
                mana.Set(5);
                var rule = Query(ruleset, state);
                Assert.IsNotNull(rule); // out of mana in forest
                rule.Execute();
            }

            {
                location.Set("desert");
                var rule = Query(ruleset, state);
                Assert.IsNull(rule); // no match
            }

            {
                health.Set(5);
                var rule = Query(ruleset, state);
                Assert.IsNotNull(rule); // out of health in desert
                rule.Execute();
            }
        }

        private static Rule Query(RulesetBuilder ruleset, StateBuilder state)
        {
            return ruleset.Build().Query(state.Build());
        }
    }
}
