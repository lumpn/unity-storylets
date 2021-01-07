public sealed class PredicateBuilder
{
    public int identifier;
    public int min, max;

    public PredicateBuilder(int identifier)
    {
        this.identifier = identifier;
    }

    public PredicateBuilder EqualTo(Lookup lookup, string value)
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
