using System;
using System.Collections.Generic;
using System.Linq;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets
{
    public sealed class State : IState
    {
        /// variables sorted by identifier
        private readonly Variable[] variables;

        public State(IEnumerable<Variable> variables)
        {
            DebugAssert.NotNull(variables);

            var array = variables.ToArray();
            Array.Sort(array, VariableIdentifierComparer.Default);
            this.variables = array;
        }

        public bool SetValue(int identifier, int value)
        {
            var idx = VariableIndex(identifier);
            if (idx < 0)
            {
                return false;
            }

            variables[idx] = new Variable(identifier, value);
            return true;
        }

        public int GetValue(int identifier)
        {
            var idx = VariableIndex(identifier);
            if (idx < 0)
            {
                return Constants.NoValue;
            }

            var variable = variables[idx];
            return variable.value;
        }

        private int VariableIndex(int identifier)
        {
            var key = new Variable(identifier, 0);
            return Array.BinarySearch<Variable>(variables, key, VariableIdentifierComparer.Default);
        }
    }
}
