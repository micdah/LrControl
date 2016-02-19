using System;
using System.Threading;

namespace micdah.LrControlApi.Common
{
    internal class StartStopThread : IDisposable
    {
        private readonly Action<Action> _iterationFunction;
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopFinishedEvent = new ManualResetEvent(false);
        private Thread _thread;

        public StartStopThread(string name, Action<Action> iterationFunction)
        {
            _iterationFunction = iterationFunction;

            if (_thread != null)
                throw new InvalidOperationException("Already started");

            _thread = new Thread(ThreadStart)
            {
                IsBackground = true,
                Name = name
            };
            _thread.Start();
        }

        public void Dispose()
        {
            if (_thread == null) return;

            _stopEvent.Set();
            _runEvent.Set();

            _stopFinishedEvent.WaitOne();

            _runEvent.Reset();
            _stopEvent.Reset();
            _stopFinishedEvent.Reset();

            _thread = null;
        }

        public void Start()
        {
            _runEvent.Set();
        }

        public void Stop()
        {
            if (_thread != null)
                throw new InvalidOperationException("Not started");

            _runEvent.Reset();
        }

        private void ThreadStart()
        {
            Action stopAction = () => _runEvent.Reset();

            while (true)
            {
                _runEvent.WaitOne();

                if (!_stopEvent.WaitOne(0))
                {
                    _iterationFunction(stopAction);
                }
                else
                {
                    break;
                }
            }

            _stopFinishedEvent.Set();
        }
    }
}