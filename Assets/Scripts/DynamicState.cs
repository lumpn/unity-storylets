using System.Collections.Generic;

namespace Lumpn.Storylets
{
    public sealed class DynamicState : IState
    {
        private readonly Dictionary<int, int> variables = new Dictionary<int, int>();

        public void SetValue(int identifier, int value)
        {
            variables[identifier] = value;
        }

        public int GetValue(int identifier)
        {
            if (variables.TryGetValue(identifier, out int value))
            {
                return value;
            }

            return Constants.NoValue;
        }
    }
}
