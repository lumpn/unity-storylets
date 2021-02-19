using NUnit.Framework;
using System;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class PredicateTest
    {
        [Test]
        public void TestOrdering()
        {
            var a = new Predicate(1, 0, 0);
            var b = new Predicate(2, 0, 0);
            var c = new Predicate(3, 0, 0);

            var cmp = PredicateIdentifierComparer.Default;
            Assert.AreEqual(0, cmp.Compare(a, a));
            Assert.AreEqual(0, cmp.Compare(b, b));
            Assert.AreEqual(0, cmp.Compare(c, c));
            Assert.Less(cmp.Compare(a, b), 0);
            Assert.Less(cmp.Compare(a, c), 0);
            Assert.Less(cmp.Compare(b, c), 0);
        }

        [Test]
        public void TestSorting()
        {
            var a = new Predicate(1, 0, 0);
            var b = new Predicate(8, 0, 0);
            var c = new Predicate(5, 0, 0);
            var d = new Predicate(2, 0, 0);
            var e = new Predicate(9, 0, 0);

            var predicates = new[] { a, b, c, d, e };
            Array.Sort(predicates, PredicateIdentifierComparer.Default);

            Assert.AreEqual(1, predicates[0].identifier);
            Assert.AreEqual(2, predicates[1].identifier);
            Assert.AreEqual(5, predicates[2].identifier);
            Assert.AreEqual(8, predicates[3].identifier);
            Assert.AreEqual(9, predicates[4].identifier);
        }

        [Test]
        public void TestMatchesValue()
        {
            var a = new Predicate(1, 0, 0);
            var b = new Predicate(2, 0, 0);
            var c = new Predicate(1, 0, 9);

            Assert.IsFalse(a.Matches(-1));
            Assert.IsFalse(b.Matches(-1));
            Assert.IsFalse(c.Matches(-1));

            Assert.IsTrue(a.Matches(0));
            Assert.IsTrue(b.Matches(0));
            Assert.IsTrue(c.Matches(0));

            Assert.IsFalse(a.Matches(9));
            Assert.IsFalse(b.Matches(9));
            Assert.IsTrue(c.Matches(9));

            Assert.IsFalse(a.Matches(10));
            Assert.IsFalse(b.Matches(10));
            Assert.IsFalse(c.Matches(10));
        }

        [Test]
        public void TestMatchesState()
        {
            var a = new Predicate(1, 0, 0);
            var b = new Predicate(2, 0, 0);
            var c = new Predicate(1, 0, 9);

            var state = new DynamicState();
            Assert.IsFalse(a.Matches(state)); // variable not found
            Assert.IsFalse(b.Matches(state));
            Assert.IsFalse(c.Matches(state));

            state.SetValue(1, 0);
            Assert.IsTrue(a.Matches(state));
            Assert.IsFalse(b.Matches(state)); // identifier mismatch
            Assert.IsTrue(c.Matches(state));

            state.SetValue(1, 1);
            Assert.IsFalse(a.Matches(state)); // out of range
            Assert.IsFalse(b.Matches(state));
            Assert.IsTrue(c.Matches(state));
        }
    }
}
