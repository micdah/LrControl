using System;
using System.Threading;
using Serilog;

namespace LrControl.Api.Common
{
    internal delegate void RequestStopHandler();

    internal delegate void IterationHandler(RequestStopHandler stop);

    /// <summary>
    /// Processing-type thread that performs a repetitive processing iteration in an
    /// endless loop until stopped.
    /// </summary>
    internal class ProcessingThread : IDisposable
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ProcessingThread>();

        private readonly IterationHandler _iterationFunction;
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(true);
        private readonly ManualResetEvent _terminatedEvent = new ManualResetEvent(false);
        private readonly Thread _thread;
        private bool _terminate;

        public ProcessingThread(string name, IterationHandler iterationFunction)
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
            void RequestStopHandler() => Stop();

            while (true)
            {
                _runEvent.WaitOne();
                _stopEvent.Reset();

                if (!_terminate)
                {
                    _iterationFunction(RequestStopHandler);
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