using System.Collections.Generic;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Tests
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
            return variables.GetOrDefault(identifier, Constants.NoValue);
        }
    }
}
