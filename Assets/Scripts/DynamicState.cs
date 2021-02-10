using System.Collections.Generic;

namespace Lumpn.Storylets
{
    public sealed class DynamicState : IState
    {
        private readonly Dictionary<int, Variable> variables = new Dictionary<int, Variable>();

        public void SetValue(int identifier, int value)
        {
            var variable = new Variable(identifier, value);
            variables[identifier] = variable;
        }

        public int GetValue(int identifier)
        {
            if (variables.TryGetValue(identifier, out Variable variable))
            {
                return variable.value;
            }

            return Constants.NoValue;
        }
    }
}
