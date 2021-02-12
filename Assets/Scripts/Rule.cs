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

        public int PredicateCount { get { return predicates.Length; } }
        public IEnumerable<Predicate> Predicates { get { return predicates; } }

        public Rule(IEnumerable<Predicate> predicates, IAction action)
        {
            Assert.NotNull(predicates);
            Assert.NotNull(action);

            var array = predicates.ToArray();
            Array.Sort(array, PredicateIdentifierComparer.Default);
            this.predicates = array;
            this.action = action;
        }

        public Predicate GetPredicate(int index)
        {
            return predicates[index];
        }

        public int PredicateIndex(int identifier)
        {
            var key = new Predicate(identifier, 0, 0);
            return Array.BinarySearch(predicates, key, PredicateIdentifierComparer.Default);
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
