using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using JetBrains.Annotations;
using Serilog;

namespace LrControl.Ui.Core
{
    public abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ViewModel>();

        protected readonly Dispatcher Dispatcher;
        private bool _disposed;
        
        protected ViewModel(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                Disposing();
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception occurred while dispogin view model");
                throw;
            }
            finally
            {
                _disposed = true;
            }
        }

        protected virtual void Disposing()
        {
            
        }

        protected void SafeInvoke(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                // Save to invoke
                action();
            }
            else
            {
                // Invoke via dispatcher
                Dispatcher.BeginInvoke(action);
            }
        } 
    }
}
