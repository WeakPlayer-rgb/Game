using System.Collections.Concurrent;

namespace NewGame
{
    public class Channel<T>
    {
        private ConcurrentQueue<T> queue = new();
    }
}