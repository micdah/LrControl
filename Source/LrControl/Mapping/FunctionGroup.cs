using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using micdah.LrControl.Annotations;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping
{
    public class FunctionGroup : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FunctionGroup));

        private static readonly List<FunctionGroup> FunctionGroups = new List<FunctionGroup>();
        private readonly LrApi _api;
        private bool _enabled;
        private ObservableCollection<ControllerFunction> _controllerFunctions;
        private bool _isGlobal;
        private Panel _panel;
        private string _key;
        private string _displayName;

        public FunctionGroup(LrApi api, Panel panel = null)
        {
            _api = api;
            ControllerFunctions = new ObservableCollection<ControllerFunction>();
            IsGlobal = panel == null;
            Panel = panel;

            FunctionGroups.Add(this);
        }

        public string DisplayName => IsGlobal ? "Global functions" : $"Functions when {Panel.Name} panel is active";

        public string Key
        {
            get { return _key; }
            set
            {
                if (value == _key) return;
                _key = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ControllerFunction> ControllerFunctions
        {
            get { return _controllerFunctions; }
            private set
            {
                if (Equals(value, _controllerFunctions)) return;
                _controllerFunctions = value;
                OnPropertyChanged();
            }
        }

        public bool IsGlobal
        {
            get { return _isGlobal; }
            private set
            {
                if (value == _isGlobal) return;
                _isGlobal = value;
                OnPropertyChanged();
            }
        }

        public Panel Panel
        {
            get { return _panel; }
            set
            {
                if (Equals(value, _panel)) return;
                _panel = value;
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

        public static FunctionGroup GetFunctionGroupFor(Panel panel)
        {
            return FunctionGroups.FirstOrDefault(@group => @group.Panel == panel);
        }

        public void Enable()
        {
            if (Enabled) return;

            if (!IsGlobal)
            {
                // Disable other non-global groups
                foreach (var group in FunctionGroups.Where(g => !g.IsGlobal && g != this))
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
                controllerFunction.Enable();
            }

            Enabled = true;
            Log.Debug($"Enabled FunctionGroup for {Panel?.Name}");
        }

        public void Disable()
        {
            if (!Enabled) return;

            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Disable();
            }

            Enabled = false;
            Log.Debug($"Disabled FunctionGroup for {Panel?.Name}");
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