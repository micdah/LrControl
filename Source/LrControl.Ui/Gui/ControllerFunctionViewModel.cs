using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using JetBrains.Annotations;
using LrControl.Core.Devices;
using LrControl.Core.Functions;
using LrControl.Core.Mapping;

namespace LrControl.Ui.Gui
{
    public class ControllerFunctionViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private ControllerFunction _controllerFunction;
        private bool _assignable;
        private Controller _controller;
        private bool _hasFunction;
        private IFunction _function;

        public ControllerFunctionViewModel(Dispatcher dispatcher, ControllerFunction controllerFunction)
        {
            _dispatcher = dispatcher;
            _controllerFunction = controllerFunction;
            Assignable = controllerFunction.Assignable;
            Controller = controllerFunction.Controller;
            HasFunction = controllerFunction.HasFunction;
            Function = controllerFunction.Function;

            _controllerFunction.PropertyChanged += ControllerFunctionOnPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Assignable
        {
            get => _assignable;
            private set
            {
                if (value == _assignable) return;
                _assignable = value;
                OnPropertyChanged();
            }
        }

        public Controller Controller
        {
            get => _controller;
            private set
            {
                if (Equals(value, _controller)) return;
                _controller = value;
                OnPropertyChanged();
            }
        }

        public bool HasFunction
        {
            get => _hasFunction;
            private set
            {
                if (value == _hasFunction) return;
                _hasFunction = value;
                OnPropertyChanged();
            }
        }

        public IFunction Function
        {
            get => _function;
            private set
            {
                if (Equals(value, _function)) return;
                _function = value;
                OnPropertyChanged();
            }
        }

        public void SetFunction(IFunction function)
        {
            _controllerFunction.Function = function;
        }

        private void ControllerFunctionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is ControllerFunction controllerFunction)) return;

            switch (e.PropertyName)
            {
                case nameof(ControllerFunction.Assignable):
                    Assignable = controllerFunction.Assignable;
                    break;
                case nameof(ControllerFunction.Controller):
                    Controller = controllerFunction.Controller;
                    break;
                case nameof(ControllerFunction.HasFunction):
                    HasFunction = controllerFunction.HasFunction;
                    break;
                case nameof(ControllerFunction.Function):
                    Function = controllerFunction.Function;
                    break;
            }
        }

        public void Dispose()
        {
            if (_controllerFunction != null)
            {
                _controllerFunction.PropertyChanged -= ControllerFunctionOnPropertyChanged;
                _controllerFunction = null;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}