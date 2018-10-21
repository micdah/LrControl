using System;
using System.Threading;
using Serilog;

namespace LrControl.LrPlugin.Api.Common
{
    /// <summary>
    /// Iteration handler, executed on each iteration while the processing thread is running
    /// </summary>
    /// <param name="cancellationToken">Cancellation token set when disposing</param>
    /// <param name="stop">Called if the iteration wishes the processing to be stopped after current execution</param>
    internal delegate void IterationHandler(CancellationToken cancellationToken, Action stop = null);

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
        private readonly Thread _thread;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private volatile bool _isRunning;
        private bool _isDisposed;

        public ProcessingThread(string name, IterationHandler iterationFunction)
        {
            _name = name;
            _iterationFunction = iterationFunction;
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = false;

            _thread = new Thread(ThreadStart)
            {
                IsBackground = true,
                Name = name
            };
            _thread.Start(_cancellationTokenSource.Token);
        }

        public void Start()
        {
            if (_isRunning) return;
            
            Log.Debug("Starting {Name}", _name);

            _stopEvent.Reset();
            _runEvent.Set();
            
            _isRunning = true;
        }
        
        public void Stop(bool wait = true)
        {
            if (!_isRunning) return;
            
            Log.Debug("Stopping {Name}", _name);

            _runEvent.Reset();
            
            if (wait)
            {
                _stopEvent.WaitOne();
            }
        }

        private void ThreadStart(object args)
        {
            var cancellationToken = (CancellationToken) args;
            var waitHandles = new[] {_runEvent, cancellationToken.WaitHandle};

            while (true)
            {
                // Wait for run-event is signaled, or cancellation token is raised
                WaitHandle.WaitAny(waitHandles);
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                        
                PerformIteration(cancellationToken);
                
                // Check if we have been stopped?
                if (!_runEvent.WaitOne(TimeSpan.Zero))
                {
                    Log.Debug("Stopped {Name}", _name);
                    _isRunning = false;
                    _stopEvent.Set();
                }
            }
        }
        
        private void PerformIteration(CancellationToken cancellationToken)
        {
            try
            {
                _iterationFunction(cancellationToken, () => Stop(false));
            }
            catch (OperationCanceledException)
            {
                // Expected if thread is disposing to quickly terminate iteration function
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception occurred in processing thread '{Name}'", _name);
            }
        }
        
        public void Dispose()
        {
            if (_isDisposed) return;
            
            Log.Debug("Disposing {Name}", _name);
            
            _cancellationTokenSource.Cancel();
            _thread.Join();
            
            _runEvent.Dispose();
            _stopEvent.Dispose();
            _cancellationTokenSource.Dispose();

            _isDisposed = true;
            Log.Debug("Disposed {Name}", _name);
        }
    }
    
}