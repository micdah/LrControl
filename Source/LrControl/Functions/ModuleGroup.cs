using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using micdah.LrControl.Annotations;
using micdah.LrControlApi.Modules.LrApplicationView;

namespace micdah.LrControl.Functions
{
    public class ModuleGroup : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ModuleGroup));

        private static readonly List<ModuleGroup> ModuleGroups = new List<ModuleGroup>();
        private readonly LrControlApi.LrControlApi _api;
        private readonly List<FunctionGroup> _lastEnabledFunctionGroups = new List<FunctionGroup>();
        private bool _enabled;
        private ObservableCollection<FunctionGroup> _functionGroups;
        private Module _module;

        public ModuleGroup(LrControlApi.LrControlApi api, Module module)
        {
            _api = api;
            Module = module;
            FunctionGroups = new ObservableCollection<FunctionGroup>();
            ModuleGroups.Add(this);
        }

        public Module Module
        {
            get { return _module; }
            private set
            {
                if (Equals(value, _module)) return;
                _module = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FunctionGroup> FunctionGroups
        {
            get { return _functionGroups; }
            private set
            {
                if (Equals(value, _functionGroups)) return;
                _functionGroups = value;
                OnPropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void EnableGroupFor(Module module)
        {
            foreach (var group in ModuleGroups)
            {
                if (group.Module == module)
                {
                    group.Enable();
                }
                else
                {
                    group.Disable();
                }
            }
        }

        private void Enable()
        {
            if (Enabled) return;

            // Enable last enabled function group(s)
            foreach (var enabledGroup in _lastEnabledFunctionGroups)
            {
                enabledGroup.Enable();
            }

            // Enable all global groups
            foreach (var globalGroup in FunctionGroups.Where(globalGroup => globalGroup.IsGlobal))
            {
                globalGroup.Enable();
            }

            Enabled = true;

            Log.Debug($"Enabled ModuleGroup for {Module.Name}");
        }


        private void Disable()
        {
            if (!Enabled) return;

            _lastEnabledFunctionGroups.Clear();
            foreach (var group in FunctionGroups)
            {
                if (group.Enabled && !group.IsGlobal)
                {
                    _lastEnabledFunctionGroups.Add(group);
                }

                group.Disable();
            }

            Enabled = false;

            Log.Debug($"Disabled ModuleGroup for {Module.Name}");
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}