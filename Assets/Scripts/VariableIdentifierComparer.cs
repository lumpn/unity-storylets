using System.Collections.Generic;

namespace Lumpn.Storylets
{
    /// Orders variables by identifier ascending
    public sealed class VariableIdentifierComparer : IComparer<Variable>
    {
        public static readonly VariableIdentifierComparer Default = new VariableIdentifierComparer();

        public int Compare(Variable a, Variable b)
        {
            return (a.identifier - b.identifier);
        }
    }
}
