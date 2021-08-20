using System.Collections.Generic;
using Lumpn.Storylets.Builders;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Tests
{
    public sealed class StateProvider : IState
    {
        private readonly Dictionary<int, IValueProvider> valueProviders = new Dictionary<int, IValueProvider>();
        private readonly SymbolLookup lookup;

        public StateProvider(SymbolLookup lookup)
        {
            this.lookup = lookup;
        }

        public void Register(IValueProvider valueProvider)
        {
            var name = valueProvider.Name;
            var identifier = lookup.Register(name);
            valueProviders.Add(identifier, valueProvider);
        }

        public int GetValue(int identifier)
        {
            if (valueProviders.TryGetValue(identifier, out IValueProvider valueProvider))
            {
                return valueProvider.Value;
            }

            return Constants.NoValue;
        }
    }
}
