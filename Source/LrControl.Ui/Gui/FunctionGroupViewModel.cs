using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using JetBrains.Annotations;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class FunctionGroupViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private FunctionGroup _functionGroup;
        private bool _enabled;
        private string _displayName;
        private bool _isGlobal;
        private ObservableCollection<ControllerFunction> _controllerFunctions;

        public FunctionGroupViewModel(Dispatcher dispatcher, FunctionGroup functionGroup)
        {
            _dispatcher = dispatcher;
            _functionGroup = functionGroup;
            Enabled = functionGroup.Enabled;
            DisplayName = functionGroup.DisplayName;
            IsGlobal = functionGroup.IsGlobal;
            ControllerFunctions = new ObservableCollection<ControllerFunction>();
            UpdateControllerFunctions(functionGroup.ControllerFunctions);

            functionGroup.PropertyChanged += FunctionGroupOnPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<ControllerFunction> ControllerFunctions
        {
            get => _controllerFunctions;
            private set
            {
                if (Equals(value, _controllerFunctions)) return;
                _controllerFunctions = value;
                OnPropertyChanged();
            }
        }

        private void FunctionGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is FunctionGroup functionGroup)) return;

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
        }

        private void UpdateControllerFunctions(IEnumerable<ControllerFunction> controllerFunctions)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(new Action(() => UpdateControllerFunctions(controllerFunctions)));
                return;
            }

            ControllerFunctions.SyncWith(controllerFunctions);
        }

        public void Dispose()
        {
            if (_functionGroup != null)
            {
                _functionGroup.PropertyChanged -= FunctionGroupOnPropertyChanged;
                _functionGroup = null;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}