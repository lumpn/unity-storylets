public sealed class VariableBuilder
{
    public int identifier;
    public int value;

    public VariableBuilder(int identifier)
    {
        this.identifier = identifier;
    }

    public VariableBuilder Set(Lookup lookup, string value)
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
