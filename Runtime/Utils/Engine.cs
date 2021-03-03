namespace Lumpn.Storylets.Utils
{
    public sealed class Engine
    {
        private const int cacheCapacity = 4096;

        private readonly IRuleset ruleset;
        private readonly StateCache cache;

        public Engine(IRuleset ruleset, IState state)
        {
            this.ruleset = ruleset;
            this.cache = new StateCache(state, cacheCapacity);
        }

        public void Execute()
        {
            cache.Invalidate();

            var rule = ruleset.Query(cache);
            rule.Execute();
        }
    }
}
