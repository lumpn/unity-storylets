using NUnit.Framework;
using System;

namespace Lumpn.Storylets.Tests
{
    [TestFixture]
    public sealed class VariableTest
    {
        [Test]
        public void TestOrdering()
        {
            var a = new Variable(1, 0);
            var b = new Variable(2, 0);
            var c = new Variable(3, 0);

            var cmp = VariableIdentifierComparer.Default;
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
            var a = new Variable(1, 0);
            var b = new Variable(8, 0);
            var c = new Variable(5, 0);
            var d = new Variable(2, 0);
            var e = new Variable(9, 0);

            var variables = new[] { a, b, c, d, e };
            Array.Sort(variables, VariableIdentifierComparer.Default);

            Assert.AreEqual(1, variables[0].identifier);
            Assert.AreEqual(2, variables[1].identifier);
            Assert.AreEqual(5, variables[2].identifier);
            Assert.AreEqual(8, variables[3].identifier);
            Assert.AreEqual(9, variables[4].identifier);
        }
    }
}
