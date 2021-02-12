using System.Collections.Generic;
using Lumpn.Storylets.Builders;

namespace Lumpn.Storylets.Tests
{
    public sealed class StateProvider : IState
    {
        private readonly Dictionary<int, IValueProvider> valueProviders = new Dictionary<int, IValueProvider>();
        private readonly Lookup lookup;

        public StateProvider(Lookup lookup)
        {
            this.lookup = lookup;
        }

        public void Register(IValueProvider valueProvider)
        {
            var name = valueProvider.name;
            var identifier = lookup.Register(name);
            valueProviders.Add(identifier, valueProvider);
        }

        public int GetValue(int identifier)
        {
            if (valueProviders.TryGetValue(identifier, out IValueProvider valueProvider))
            {
                return valueProvider.value;
            }

            return Constants.NoValue;
        }
    }
}
