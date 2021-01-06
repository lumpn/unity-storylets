public struct Predicate
{
    public readonly int identifier;
    public readonly int min, max;

    public Predicate(int identifier, int min, int max)
    {
        this.identifier = identifier;
        this.min = min;
        this.max = max;
    }

    public bool Evaluate(int value)
    {
        return (value >= min)
            && (value <= max);
    }

    public bool Matches(State state)
    {
        var value = state.GetValue(identifier);
        return Evaluate(value);
    }
}
