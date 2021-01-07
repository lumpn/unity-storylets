public sealed class PredicateBuilder
{
    private readonly Lookup lookup;
    public int identifier;
    public int min, max;

    public PredicateBuilder(Lookup lookup, int identifier)
    {
        this.lookup = lookup;
        this.identifier = identifier;
    }

    public PredicateBuilder EqualTo(string value)
    {
        return EqualTo(lookup.Register(value));
    }

    public PredicateBuilder EqualTo(int value)
    {
        Assert.NotEqual(value, Constants.noValue);

        min = value;
        max = value;
        return this;
    }

    public PredicateBuilder LessThan(int value)
    {
        min = Constants.minValue;
        max = value - 1;
        return this;
    }

    public Predicate Build()
    {
        return new Predicate(identifier, min, max);
    }
}
