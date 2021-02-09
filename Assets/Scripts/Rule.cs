using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets
{
    public sealed class Rule
    {
        /// predicates sorted by identifier
        private readonly Predicate[] predicates;
        private readonly IEffect effect;

        public int predicateCount { get { return predicates.Length; } }
        public Predicate[] Predicates { get { return predicates; } }

        public Rule(Predicate[] predicates, IEffect effect)
        {
            Assert.NotNull(predicates);
            Assert.NotNull(effect);

            this.predicates = predicates;
            this.effect = effect;
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
            effect.Apply();
        }
    }
}
