using System.Collections.Generic;

public sealed class VariableIdentifierComparer : IComparer<Variable>
{
    public static readonly VariableIdentifierComparer Default = new VariableIdentifierComparer();

    public int Compare(Variable a, Variable b)
    {
        return (a.identifier - b.identifier); // TODO Jonas: or the other way around?
    }
}
