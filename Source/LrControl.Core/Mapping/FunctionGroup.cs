using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api;
using LrControl.Api.Modules.LrDevelopController;
using Serilog;

namespace LrControl.Core.Mapping
{
    public class FunctionGroup : INotifyPropertyChanged
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<FunctionGroup>();

        private static readonly List<FunctionGroup> AllFunctionGroups = new List<FunctionGroup>();
        private readonly LrApi _api;
        private bool _enabled;
        private ObservableCollection<ControllerFunction> _controllerFunctions;
        private bool _isGlobal;
        private Panel _panel;
        private string _key;

        public FunctionGroup(LrApi api, Panel panel = null)
        {
            _api = api;
            ControllerFunctions = new ObservableCollection<ControllerFunction>();
            IsGlobal = panel == null;
            Panel = panel;

            AllFunctionGroups.Add(this);
        }

        public string DisplayName => IsGlobal ? "Global" : $"{Panel.Name}";

        public string Key
        {
            get => _key;
            set
            {
                if (value == _key) return;
                _key = value;
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

        public Panel Panel
        {
            get => _panel;
            private set
            {
                if (Equals(value, _panel)) return;
                _panel = value;
                OnPropertyChanged();
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public static FunctionGroup GetFunctionGroupFor(Panel panel)
        {
            return AllFunctionGroups.FirstOrDefault(group => group.Panel == panel);
        }

        public void Enable()
        {
            if (!IsGlobal)
            {
                // Disable other non-global groups
                foreach (var group in AllFunctionGroups.Where(g => !g.IsGlobal && g != this))
                {
                    group.Disable();
                }

                // Switch to panel/module
                if (Panel != null)
                {
                    _api.LrDevelopController.RevealPanel(Panel);
                }
            }

            // Enable group
            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Enable(!IsGlobal);
            }

            Enabled = true;
            Log.Debug("Enabled FunctionGroup for {Name}", Panel?.Name);
        }

        public void Disable()
        {
            if (!Enabled) return;

            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Disable();
            }

            Enabled = false;
            Log.Debug("Disabled FunctionGroup for {Name}", Panel?.Name);
        }

        public void ClearControllerFunctions()
        {
            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Dispose();
            }

            ControllerFunctions.Clear();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}