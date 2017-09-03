using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class FunctionGroupManagerViewModel : ViewModel
    {
        private readonly FunctionGroupManager _functionGroupManager;
        private ObservableCollection<ModuleGroupViewModel> _modules;

        public FunctionGroupManagerViewModel(Dispatcher dispatcher, FunctionGroupManager functionGroupManager) :
            base(dispatcher)
        {
            _functionGroupManager = functionGroupManager;
            Modules = new ObservableCollection<ModuleGroupViewModel>();
            UpdateModules(functionGroupManager.Modules);

            functionGroupManager.PropertyChanged += FunctionGroupManagerOnPropertyChanged;
        }

        public ObservableCollection<ModuleGroupViewModel> Modules
        {
            get => _modules;
            private set
            {
                if (Equals(value, _modules)) return;
                _modules = value;
                OnPropertyChanged();
            }
        }

        private void FunctionGroupManagerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is FunctionGroupManager functionGroupManager)) return;

            SafeInvoke(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(FunctionGroupManager.Modules):
                        UpdateModules(functionGroupManager.Modules);
                        break;
                }
            });
        }

        private void UpdateModules(IEnumerable<ModuleGroup> modules)
        {
            Modules.SyncWith(modules.Select(m => new ModuleGroupViewModel(Dispatcher, m)));
        }

        protected override void Disposing()
        {
            _functionGroupManager.PropertyChanged -= FunctionGroupManagerOnPropertyChanged;
        }
    }
}