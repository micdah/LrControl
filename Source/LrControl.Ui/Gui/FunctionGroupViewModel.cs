using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class FunctionGroupViewModel : ViewModel
    {
        private readonly FunctionGroup _functionGroup;
        private ObservableCollection<ControllerFunctionViewModel> _controllerFunctions;
        private string _displayName;
        private bool _enabled;
        private bool _isGlobal;

        public FunctionGroupViewModel(Dispatcher dispatcher, FunctionGroup functionGroup) : base(dispatcher)
        {
            _functionGroup = functionGroup;
            Enabled = functionGroup.Enabled;
            DisplayName = functionGroup.DisplayName;
            IsGlobal = functionGroup.IsGlobal;
            ControllerFunctions = new ObservableCollection<ControllerFunctionViewModel>();
            UpdateControllerFunctions(functionGroup.ControllerFunctions);

            functionGroup.PropertyChanged += FunctionGroupOnPropertyChanged;
        }

        public bool Enabled
        {
            get => _enabled;
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get => _displayName;
            private set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public bool IsGlobal
        {
            get => _isGlobal;
            private set
            {
                if (value == _isGlobal) return;
                _isGlobal = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ControllerFunctionViewModel> ControllerFunctions
        {
            get => _controllerFunctions;
            private set
            {
                if (Equals(value, _controllerFunctions)) return;
                _controllerFunctions?.DisposeAndClear();
                _controllerFunctions = value;
                OnPropertyChanged();
            }
        }

        private void FunctionGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is FunctionGroup functionGroup)) return;

            SafeInvoke(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(FunctionGroup.Enabled):
                        Enabled = functionGroup.Enabled;
                        break;
                    case nameof(FunctionGroup.DisplayName):
                        DisplayName = functionGroup.DisplayName;
                        break;
                    case nameof(FunctionGroup.IsGlobal):
                        IsGlobal = functionGroup.IsGlobal;
                        break;
                    case nameof(FunctionGroup.ControllerFunctions):
                        UpdateControllerFunctions(functionGroup.ControllerFunctions);
                        break;
                }
            });
        }

        private void UpdateControllerFunctions(IEnumerable<ControllerFunction> controllerFunctions)
        {
            ControllerFunctions.SyncWith(
                controllerFunctions.Select(x => new ControllerFunctionViewModel(Dispatcher, x)).ToList());
        }

        protected override void Disposing()
        {
            _functionGroup.PropertyChanged -= FunctionGroupOnPropertyChanged;
            ControllerFunctions = null;
        }
    }
}