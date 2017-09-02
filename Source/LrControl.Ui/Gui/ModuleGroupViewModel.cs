using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using JetBrains.Annotations;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Devices;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class ModuleGroupViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private ModuleGroup _moduleGroup;
        private bool _enabled;
        private Module _module;
        private ObservableCollection<FunctionGroupViewModel> _functionGroups;

        public ModuleGroupViewModel(Dispatcher dispatcher, ModuleGroup moduleGroup)
        {
            _dispatcher = dispatcher;
            _moduleGroup = moduleGroup;
            Enabled = moduleGroup.Enabled;
            Module = moduleGroup.Module;
            FunctionGroups = new ObservableCollection<FunctionGroupViewModel>();
            UpdateFunctionGroups(moduleGroup.FunctionGroups);

            moduleGroup.PropertyChanged += ModuleGroupOnPropertyChanged;
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

        public Module Module
        {
            get => _module;
            private set
            {
                if (Equals(value, _module)) return;
                _module = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FunctionGroupViewModel> FunctionGroups
        {
            get => _functionGroups;
            private set
            {
                if (Equals(value, _functionGroups)) return;
                _functionGroups = value;
                OnPropertyChanged();
            }
        }

        private void ModuleGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is ModuleGroup moduleGroup)) return;

            switch (e.PropertyName)
            {
                case nameof(ModuleGroup.Enabled):
                    Enabled = moduleGroup.Enabled;
                    break;
                case nameof(ModuleGroup.Module):
                    Module = moduleGroup.Module;
                    break;
                case nameof(ModuleGroup.FunctionGroups):
                    UpdateFunctionGroups(moduleGroup.FunctionGroups);
                    break;
            }
        }

        private void UpdateFunctionGroups(IEnumerable<FunctionGroup> functionGroups)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(new Action(() => UpdateFunctionGroups(functionGroups)));
                return;
            }

            FunctionGroups.SyncWith(functionGroups.Select(f => new FunctionGroupViewModel(_dispatcher, f)));
        }

        public void Dispose()
        {
            if (_moduleGroup != null)
            {
                _moduleGroup.PropertyChanged -= ModuleGroupOnPropertyChanged;
                _moduleGroup = null;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanAssignFunction(Controller controllerFunctionController, bool functionGroupIsGlobal)
        {
            return _moduleGroup.CanAssignFunction(controllerFunctionController, functionGroupIsGlobal);
        }

        public void RecalculateControllerFunctionState()
        {
            _moduleGroup.RecalculateControllerFunctionState();
        }
    }
}