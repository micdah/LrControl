using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Core.Device;
using LrControl.Core.Device.Enums;
using LrControl.Core.Functions;

namespace LrControl.Core.Mapping
{
    public class ControllerFunction : INotifyPropertyChanged, IDisposable
    {
        private Controller _controller;
        private IFunction _function;
        private bool _assignable;

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

        public IFunction Function
        {
            get { return _function; }
            set
            {
                if (Equals(value, _function)) return;

                _function?.Disable();

                _function = value;

                if (_function != null)
                {
                    _function.Controller = _controller;
                    if (Enabled) _function.Enable();
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFunction));
            }
        }

        public bool Assignable
        {
            get { return _assignable; }
            set
            {
                if (value == _assignable) return;
                _assignable = value;
                OnPropertyChanged();
            }
        }

        private bool Enabled { get; set; }

        public bool HasFunction => Function != null;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public void Enable(bool resetIfUnmapped)
        {
            if (Function != null)
            {
                Function.Enable();
            }
            else if(resetIfUnmapped)
            {
                if (Controller.ControllerType == ControllerType.Encoder ||
                    Controller.ControllerType == ControllerType.Fader)
                {
                    Controller.Reset();
                }
            }

            Enabled = true;
        }

        public void Disable()
        {
            Function?.Disable();
            Enabled = false;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}