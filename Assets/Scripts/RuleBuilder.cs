using System.Collections.Generic;
using System.Linq;

public sealed class RuleBuilder
{
    private readonly List<PredicateBuilder> predicateBuilders = new List<PredicateBuilder>();
    public IEffect effect;

    public RuleBuilder(IEffect effect)
    {
        this.effect = effect;
    }

    public PredicateBuilder AddPredicate(Lookup lookup, string identifier)
    {
        return AddPredicate(lookup.Register(identifier));
    }

    public PredicateBuilder AddPredicate(int identifier)
    {
        var builder = new PredicateBuilder(identifier);
        predicateBuilders.Add(builder);
        return builder;
    }

    public RuleBuilder SetEffect(IEffect effect)
    {
        this.effect = effect;
        return this;
    }

    public Rule Build()
    {
        var predicates = predicateBuilders.Select(p => p.Build())
                                          .OrderBy(p => p, PredicateIdentifierComparer.Default)
                                          .ToArray();

        return new Rule(predicates, effect);
    }
}
