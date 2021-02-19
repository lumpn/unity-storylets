using NUnit.Framework;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class StateTest
    {
        [Test]
        public void TestGetValue()
        {
            var a = new Variable(1, 1);
            var b = new Variable(8, 2);
            var c = new Variable(5, 3);
            var d = new Variable(2, 4);
            var e = new Variable(9, 5);
            var variables = new[] { a, b, c, d, e };

            var state = new State(variables);
            foreach (var variable in variables)
            {
                Assert.AreEqual(variable.value, state.GetValue(variable.identifier));
            }

            Assert.AreEqual(Constants.NoValue, state.GetValue(100));
        }

        [Test]
        public void TestSetValue()
        {
            var variable = new Variable(1, 1);
            var state = new State(new[] { variable });

            Assert.AreEqual(variable.value, state.GetValue(variable.identifier));

            var set1 = state.SetValue(1, 2);
            Assert.IsTrue(set1);
            Assert.AreEqual(2, state.GetValue(1));

            var set2 = state.SetValue(2, 0);
            Assert.IsFalse(set2);
            Assert.AreEqual(Constants.NoValue, state.GetValue(2));
        }
    }
}
