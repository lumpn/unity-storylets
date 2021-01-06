using System.Collections;
using System.Collections.Generic;

public sealed class PredicateIdentifierComparer : IComparer<Predicate>
{
    public static readonly PredicateIdentifierComparer Default = new PredicateIdentifierComparer();

    public int Compare(Predicate a, Predicate b)
    {
        return a.identifier - b.identifier; // TODO Jonas: or the other way around?
    }
}
