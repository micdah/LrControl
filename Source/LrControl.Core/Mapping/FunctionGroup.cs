using System.Collections.Generic;
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
        private readonly List<ControllerFunction> _controllerFunctions;
        private bool _enabled;
        private bool _isGlobal;
        private Panel _panel;
        private string _key;

        internal FunctionGroup(LrApi api, Panel panel = null)
        {
            _api = api;
            IsGlobal = panel == null;
            Panel = panel;

            _controllerFunctions = new List<ControllerFunction>();
            OnPropertyChanged(nameof(ControllerFunctions));

            AllFunctionGroups.Add(this);
        }

        public string DisplayName => IsGlobal ? "Global" : $"{Panel.Name}";
        public IEnumerable<ControllerFunction> ControllerFunctions => _controllerFunctions;

        public string Key
        {
            get => _key;
            internal set
            {
                if (value == _key) return;
                _key = value;
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

        internal static FunctionGroup GetFunctionGroupFor(Panel panel)
        {
            return AllFunctionGroups.FirstOrDefault(group => group.Panel == panel);
        }

        internal void Enable()
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

        internal void Disable()
        {
            if (!Enabled) return;

            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Disable();
            }

            Enabled = false;
            Log.Debug("Disabled FunctionGroup for {Name}", Panel?.Name);
        }

        internal void AddControllerFunction(ControllerFunction controllerFunction)
        {
            _controllerFunctions.Add(controllerFunction);
            OnPropertyChanged(nameof(ControllerFunctions));
        }

        internal void ClearControllerFunctions()
        {
            foreach (var controllerFunction in _controllerFunctions)
            {
                controllerFunction.Dispose();
            }

            _controllerFunctions.Clear();
            OnPropertyChanged(nameof(ControllerFunctions));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}