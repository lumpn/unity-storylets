using System.Collections.Generic;
using System.Linq;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Builders
{
    public sealed class RuleBuilder
    {
        private readonly SymbolLookup lookup;
        private readonly List<PredicateBuilder> predicateBuilders = new List<PredicateBuilder>();
        public IAction action;

        public RuleBuilder(SymbolLookup lookup, IAction action)
        {
            DebugAssert.NotNull(lookup);
            DebugAssert.NotNull(action);

            this.lookup = lookup;
            this.action = action;
        }

        public PredicateBuilder AddPredicate(string identifier)
        {
            return AddPredicate(lookup.Register(identifier));
        }

        public PredicateBuilder AddPredicate(int identifier)
        {
            // TODO Jonas: disallow existing identifier
            var builder = new PredicateBuilder(lookup, identifier);
            predicateBuilders.Add(builder);
            return builder;
        }

        public RuleBuilder SetAction(IAction action)
        {
            DebugAssert.NotNull(action);

            this.action = action;
            return this;
        }

        public Rule Build()
        {
            // TODO Jonas: filter distinct?
            var predicates = predicateBuilders.Select(p => p.Build());
            return new Rule(predicates, action);
        }
    }
}
