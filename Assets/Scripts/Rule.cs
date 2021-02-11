using Lumpn.Storylets.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Lumpn.Storylets
{
    public sealed class Rule
    {
        /// predicates sorted by identifier
        private readonly Predicate[] predicates;
        private readonly IAction action;

        public int predicateCount { get { return predicates.Length; } }
        public Predicate[] Predicates { get { return predicates; } }

        public Rule(IEnumerable<Predicate> predicates, IAction action)
        {
            Assert.NotNull(predicates);
            Assert.NotNull(action);

            var array = predicates.ToArray();
            Array.Sort(array, PredicateIdentifierComparer.Default);
            this.predicates = array;
            this.action = action;
        }

        public bool Matches(IState state)
        {
            foreach (var predicate in predicates)
            {
                if (!predicate.Matches(state))
                {
                    return false;
                }
            }
            return true;
        }

        public void Execute()
        {
            action.Execute();
        }
    }
}
