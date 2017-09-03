using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class ModuleGroupViewModel : ViewModel
    {
        private readonly ModuleGroup _moduleGroup;
        private bool _enabled;
        private ObservableCollection<FunctionGroupViewModel> _functionGroups;
        private Module _module;

        public ModuleGroupViewModel(Dispatcher dispatcher, ModuleGroup moduleGroup) : base(dispatcher)
        {
            _moduleGroup = moduleGroup;
            Enabled = moduleGroup.Enabled;
            Module = moduleGroup.Module;
            FunctionGroups = new ObservableCollection<FunctionGroupViewModel>();
            UpdateFunctionGroups(moduleGroup.FunctionGroups);

            moduleGroup.PropertyChanged += ModuleGroupOnPropertyChanged;
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
                _functionGroups?.DisposeAndClear();
                _functionGroups = value;
                OnPropertyChanged();
            }
        }

        private void ModuleGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is ModuleGroup moduleGroup)) return;

            SafeInvoke(() =>
            {
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
            });
        }

        private void UpdateFunctionGroups(IEnumerable<FunctionGroup> functionGroups)
        {
            FunctionGroups.SyncWith(functionGroups.Select(x => new FunctionGroupViewModel(Dispatcher, x)).ToList());
        }

        public bool CanAssignFunction(ControllerViewModel controllerVm, bool functionGroupIsGlobal)
        {
            return controllerVm.CanAssignFunction(_moduleGroup, functionGroupIsGlobal);
        }

        public void RecalculateControllerFunctionState()
        {
            _moduleGroup.RecalculateControllerFunctionState();
        }

        protected override void Disposing()
        {
            _moduleGroup.PropertyChanged -= ModuleGroupOnPropertyChanged;
            FunctionGroups = null;
        }
    }
}