using System;
using System.Threading;
using Serilog;

namespace LrControl.Api.Common
{
    /// <summary>
    /// Iteration handler, executed on each iteration while the processing thread is running
    /// </summary>
    /// <param name="stop">Called if the iteration wishes the processing to be stopped after current execution</param>
    internal delegate void IterationHandler(Action stop = null);

    /// <summary>
    /// Processing-type thread that performs a repetitive processing iteration in an
    /// endless loop until stopped.
    /// </summary>
    internal class ProcessingThread : IDisposable
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ProcessingThread>();

        private readonly string _name;
        private readonly IterationHandler _iterationFunction;
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(true);
        private volatile ProcessingThreadState _state;

        public ProcessingThread(string name, IterationHandler iterationFunction)
        {
            _name = name;
            _iterationFunction = iterationFunction;
            _state = ProcessingThreadState.Stopped;

            var thread = new Thread(ThreadStart)
            {
                IsBackground = true,
                Name = name
            };
            thread.Start();
        }

        public void Start()
        {
            if (_state == ProcessingThreadState.Started) return;
            
            Log.Debug("Starting {Name}", _name);

            _state = ProcessingThreadState.Started;
            
            _runEvent.Set();    // Unblock iteration thread
        }

        public void Stop()
        {
            if (_state == ProcessingThreadState.Stopped) return;
            
            Log.Debug("Stopping {Name}", _name);

            _state = ProcessingThreadState.Stopped;
            
            _runEvent.Reset();       // Stop executing iteration function        
            _stopEvent.WaitOne();    // Wait for last iteration to complete
            
            Log.Debug("Stopped {Name}", _name);
        }

        public void Dispose()
        {
            if (_state == ProcessingThreadState.Terminated) return;

            Log.Debug("Terminating {Name}", _name);
            
            _state = ProcessingThreadState.Terminated;

            _stopEvent.Reset();    // Ensure stop event is not set (if already stopped)
            _runEvent.Set();       // Unblock iteration thread so that it can discover new state and terminate
            _stopEvent.WaitOne();  // Wait for iteration thread to terminate
            
            // Dispose events
            _runEvent.Dispose();
            _stopEvent.Dispose();

            Log.Debug("Terminated {Name}", _name);
        }

        private void ThreadStart()
        {
            void StopAfterIteration()
            {
                _state = ProcessingThreadState.Stopped;
                _runEvent.Reset();
            }
            
        Loop:
            // Wait for thread to be started
            _runEvent.WaitOne();
            _stopEvent.Reset();
            
            // Check requested state and act accordingly
            if (_state == ProcessingThreadState.Started)
            {
                try
                {
                    _iterationFunction(StopAfterIteration);
                }
                catch (Exception e)
                {
                    Log.Error(e,
                        "Exception occurred while executing iteration function for processing thread '{Name}'",
                        _name);
                }
            }
            
            _stopEvent.Set();

            if (_state != ProcessingThreadState.Terminated)
                goto Loop;
        }
    }
    
}