using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControl.Mapping.Functions;

namespace micdah.LrControl.Mapping
{
    public class ControllerFunction : INotifyPropertyChanged, IDisposable
    {
        private Controller _controller;
        private Function _function;

        public Controller Controller
        {
            get { return _controller; }
            set
            {
                if (Equals(value, _controller)) return;
                _controller = value;

                if (_function != null)
                    _function.Controller = value;

                OnPropertyChanged();
            }
        }

        public Function Function
        {
            get { return _function; }
            set
            {
                if (Equals(value, _function)) return;

                _function?.Disable();

                _function = value;
                _function.Controller = _controller;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Enable()
        {
            Function?.Enable();
        }

        public void Disable()
        {
            Function?.Disable();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _controller = null;

            if (_function != null)
            {
                _function.Disable();
                _function.Controller = null;
                _function = null;
            }
        }
    }
}