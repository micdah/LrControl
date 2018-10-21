using System.ComponentModel;
using System.Windows.Threading;
using LrControl.Core.Functions;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class ControllerFunctionViewModel : ViewModel
    {
        private readonly ControllerFunction _controllerFunction;
        private bool _assignable;
        private ControllerViewModel _controller;
        private bool _hasFunction;
        private FunctionViewModel _function;

        public ControllerFunctionViewModel(Dispatcher dispatcher, ControllerFunction controllerFunction) : base(dispatcher)
        {
            _controllerFunction = controllerFunction;
            Assignable = controllerFunction.Assignable;
            Controller = new ControllerViewModel(dispatcher, controllerFunction.Controller);
            HasFunction = controllerFunction.HasFunction;
            Function = controllerFunction.Function != null
                ? new FunctionViewModel(dispatcher, controllerFunction.Function)
                : null;

            _controllerFunction.PropertyChanged += ControllerFunctionOnPropertyChanged;
        }

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

        public ControllerViewModel Controller
        {
            get => _controller;
            private set
            {
                if (Equals(value, _controller)) return;
                _controller?.Dispose();
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

        public FunctionViewModel Function
        {
            get => _function;
            private set
            {
                if (Equals(value, _function)) return;
                _function?.Dispose();
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

            SafeInvoke(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(ControllerFunction.Assignable):
                        Assignable = controllerFunction.Assignable;
                        break;
                    case nameof(ControllerFunction.Controller):
                        //Controller = new ControllerViewModel(Dispatcher, controllerFunction.Controller);
                        break;
                    case nameof(ControllerFunction.HasFunction):
                        HasFunction = controllerFunction.HasFunction;
                        break;
                    case nameof(ControllerFunction.Function):
                        Function = controllerFunction.Function != null
                            ? new FunctionViewModel(Dispatcher, controllerFunction.Function)
                            : null;
                        break;
                }
            });
        }

        protected override void Disposing()
        {
            _controllerFunction.PropertyChanged -= ControllerFunctionOnPropertyChanged;
            Controller = null;
        }
    }
}