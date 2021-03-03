using NUnit.Framework;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class StateCacheTest
    {
        [Test]
        public void TestGetValue()
        {
            var state = new DynamicState();
            var cache = new StateCache(state, 4096);

            state.SetValue(1, 10);
            Assert.AreEqual(10, cache.GetValue(1)); // cache reflects state

            // change state without invalidating cache
            state.SetValue(1, 20);
            Assert.AreEqual(10, cache.GetValue(1)); // cache reports old value

            // invalidate cache
            cache.Invalidate();
            Assert.AreEqual(20, cache.GetValue(1)); // cache reports new value
        }
    }
}
