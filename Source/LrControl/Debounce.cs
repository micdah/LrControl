using System;
using System.Threading;

namespace micdah.LrControl
{
    public static class Debounce
    {
        public static Action Action(Action action, int delay)
        {
            var abortEvent = new ManualResetEvent(false);

            return () =>
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    // Abort other waiting threads
                    abortEvent.Set();
                    abortEvent.Reset();

                    if (!abortEvent.WaitOne(delay))
                    {
                        action();
                    }
                });
            };
        }

        public static Action<T> Action<T>(Action<T> action, int delay)
        {
            var abortEvent = new ManualResetEvent(false);

            return t =>
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    // Abort other waiting threads
                    abortEvent.Set();
                    abortEvent.Reset();

                    if (!abortEvent.WaitOne(delay))
                    {
                        action(t);
                    }
                });
            };
        }
    }
}