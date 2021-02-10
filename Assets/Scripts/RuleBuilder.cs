using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Storylets
{
    public sealed class RuleBuilder
    {
        private readonly Lookup lookup;
        private readonly List<PredicateBuilder> predicateBuilders = new List<PredicateBuilder>();
        public IAction action;

        public RuleBuilder(Lookup lookup, IAction action)
        {
            this.lookup = lookup;
            this.action = action;
        }

        public PredicateBuilder AddPredicate(string identifier)
        {
            return AddPredicate(lookup.Register(identifier));
        }

        public PredicateBuilder AddPredicate(int identifier)
        {
            var builder = new PredicateBuilder(lookup, identifier);
            predicateBuilders.Add(builder);
            return builder;
        }

        public RuleBuilder SetAction(IAction action)
        {
            this.action = action;
            return this;
        }

        public Rule Build()
        {
            var predicates = predicateBuilders.Select(p => p.Build())
                                              .OrderBy(p => p, PredicateIdentifierComparer.Default)
                                              .ToArray();

            return new Rule(predicates, action);
        }
    }
}
