using System.Collections.Generic;

namespace Lumpn.Storylets
{
    /// orders predicates by identifier ascending
    public sealed class PredicateIdentifierComparer : IComparer<Predicate>
    {
        public static readonly PredicateIdentifierComparer Default = new PredicateIdentifierComparer();

        public int Compare(Predicate a, Predicate b)
        {
            return (a.identifier - b.identifier);
        }
    }
}
