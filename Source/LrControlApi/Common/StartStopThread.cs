using System;
using System.Threading;
using Serilog;

namespace micdah.LrControlApi.Common
{
    internal delegate void RequestStopHandler();

    internal delegate void IterationHandler(RequestStopHandler stop);

    internal class StartStopThread : IDisposable
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<StartStopThread>();

        private readonly IterationHandler _iterationFunction;
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(true);
        private readonly ManualResetEvent _terminatedEvent = new ManualResetEvent(false);
        private readonly Thread _thread;
        private bool _terminate;

        public StartStopThread(string name, IterationHandler iterationFunction)
        {
            _iterationFunction = iterationFunction;

            _thread = new Thread(ThreadStart)
            {
                IsBackground = true,
                Name = name
            };
            _thread.Start();
        }

        public void Start()
        {
            Log.Debug("Start");
            _runEvent.Set();
        }

        public void Stop(bool wait = false)
        {
            Log.Debug("Stop");
            _runEvent.Reset();

            if (wait)
            {
                _stopEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            if (_thread == null) return;

            // Stop thread
            _terminate = true;
            _runEvent.Set();
            _terminatedEvent.WaitOne();

            _runEvent.Dispose();
            _stopEvent.Dispose();
            _terminatedEvent.Dispose();
        }

        private void ThreadStart()
        {
            RequestStopHandler stop = () => Stop();

            while (true)
            {
                _runEvent.WaitOne();
                _stopEvent.Reset();

                if (!_terminate)
                {
                    _iterationFunction(stop);
                }
                else
                {
                    break;
                }

                _stopEvent.Set();
            }

            _terminatedEvent.Set();
        }
    }
}