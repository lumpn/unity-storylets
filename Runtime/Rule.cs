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

        public Rule(IEnumerable<Predicate> predicates, IAction action)
        {
            DebugAssert.NotNull(predicates);
            DebugAssert.NotNull(action);

            var array = predicates.ToArray();
            Array.Sort(array, PredicateIdentifierComparer.Default);
            this.predicates = array;
            this.action = action;
        }

        public bool TryGetPredicate(int identifier, out Predicate predicate)
        {
            var key = new Predicate(identifier, 0, 0);
            var idx = Array.BinarySearch(predicates, key, PredicateIdentifierComparer.Default);
            if (idx >= 0)
            {
                predicate = predicates[idx];
                return true;
            }

            predicate = key;
            return false;
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

        public static IEnumerable<Predicate> GetPredicates(IEnumerable<Rule> rules)
        {
            return rules.SelectMany(p => p.predicates);
        }
    }
}
