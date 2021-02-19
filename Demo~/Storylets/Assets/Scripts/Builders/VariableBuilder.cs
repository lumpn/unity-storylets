namespace Lumpn.Storylets.Builders
{
    public sealed class VariableBuilder
    {
        private readonly SymbolLookup lookup;
        public int identifier;
        public int value;

        public VariableBuilder(SymbolLookup lookup, int identifier)
        {
            this.lookup = lookup;
            this.identifier = identifier;
        }

        public VariableBuilder Set(string value)
        {
            return Set(lookup.Register(value));
        }

        public VariableBuilder Set(int value)
        {
            this.value = value;
            return this;
        }

        public Variable Build()
        {
            return new Variable(identifier, value);
        }
    }
}
