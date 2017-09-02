using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using JetBrains.Annotations;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui
{
    public class FunctionGroupManagerViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private FunctionGroupManager _functionGroupManager;
        private ObservableCollection<ModuleGroupViewModel> _modules;
        public event PropertyChangedEventHandler PropertyChanged;

        public FunctionGroupManagerViewModel(Dispatcher dispatcher, FunctionGroupManager functionGroupManager)
        {
            _dispatcher = dispatcher;
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

            switch (e.PropertyName)
            {
                case nameof(FunctionGroupManager.Modules):
                    UpdateModules(functionGroupManager.Modules);
                    break;
            }
        }

        private void UpdateModules(IEnumerable<ModuleGroup> modules)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(new Action(() => UpdateModules(modules)));
                return;
            }

            foreach (var module in Modules)
            {
                module.Dispose();
            }
            Modules.Clear();
            Modules.AddRange(modules.Select(m => new ModuleGroupViewModel(_dispatcher, m)));
            
        }

        public void Dispose()
        {
            if (_functionGroupManager != null)
            {
                _functionGroupManager.PropertyChanged -= FunctionGroupManagerOnPropertyChanged;
                _functionGroupManager = null;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}