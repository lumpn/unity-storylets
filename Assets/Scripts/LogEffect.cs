using UnityEngine;

namespace Lumpn.Storylets
{
    public sealed class LogEffect : IEffect
    {
        private readonly string message;

        public LogEffect(string message)
        {
            this.message = message;
        }

        public void Apply()
        {
            Debug.Log(message);
        }
    }
}
