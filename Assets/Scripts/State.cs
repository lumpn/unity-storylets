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
            Assert.NotNull(variables);

            var array = variables.ToArray();
            Array.Sort(array, VariableIdentifierComparer.Default);
            this.variables = array;
        }

        public int GetValue(int identifier)
        {
            var key = new Variable(identifier, 0);
            var idx = Array.BinarySearch<Variable>(variables, key, VariableIdentifierComparer.Default);
            if (idx < 0)
            {
                return Constants.NoValue;
            }

            var variable = variables[idx];
            return variable.value;
        }
    }
}
