using UnityEngine;

namespace Lumpn.Storylets.Tests
{
    public sealed class LogAction : IAction
    {
        private readonly string message;

        public LogAction(string message)
        {
            this.message = message;
        }

        public void Execute()
        {
            Debug.Log(message);
        }
    }
}
