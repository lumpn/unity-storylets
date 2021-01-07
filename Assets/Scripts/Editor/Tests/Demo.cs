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
            var ruleset = new RulesetBuilder();

            var lowHealth = ruleset.AddRule(new LogEffect("Bob could use some water here!"));
            lowHealth.AddPredicate(lookup, "location").EqualTo(lookup, "desert");
            lowHealth.AddPredicate(lookup, "bob/health").LessThan(10);

            var lowMana = ruleset.AddRule(new LogEffect("Anybody got some raspberries for bob?"));
            lowMana.AddPredicate(lookup, "location").EqualTo(lookup, "forest");
            lowMana.AddPredicate(lookup, "bob/mana").LessThan(10);

            var state = new StateBuilder();
            var location = state.AddVariable(lookup, "location").Set(lookup, "forest");
            var health = state.AddVariable(lookup, "bob/health").Set(100);
            var mana = state.AddVariable(lookup, "bob/mana").Set(100);

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
                location.Set(lookup, "desert");
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
