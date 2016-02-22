using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using micdah.LrControl.Annotations;
using micdah.LrControlApi.Modules.LrDevelopController;
using Midi.Devices;
using Midi.Messages;

namespace micdah.LrControl.Mapping.Functions
{
    public class FunctionGroup : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FunctionGroup));

        private static readonly List<FunctionGroup> FunctionGroups = new List<FunctionGroup>();
        private readonly LrControlApi.LrControlApi _api;
        private bool _enabled;
        private ObservableCollection<Function> _functions;
        private IInputDevice _inputDevice;
        private bool _isGlobal;
        private Panel _panel;

        public FunctionGroup(LrControlApi.LrControlApi api, Panel panel) : this(api, false)
        {
            Panel = panel;
        }

        public FunctionGroup(LrControlApi.LrControlApi api) : this(api, true)
        {
        }

        private FunctionGroup(LrControlApi.LrControlApi api, bool isGlobal)
        {
            _api = api;
            Functions = new ObservableCollection<Function>();
            FunctionGroups.Add(this);
            IsGlobal = isGlobal;
        }

        public ObservableCollection<Function> Functions
        {
            get { return _functions; }
            private set
            {
                if (Equals(value, _functions)) return;
                _functions = value;
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
                    _api.LrDialogs.ShowBezel($"Active panel: {Panel.Name}");
                }
            }

            // Enable group
            foreach (var function in Functions)
            {
                function.Enable();
            }

            Enabled = true;
            Log.Debug($"Enabled FunctionGroup for {Panel?.Name}");
        }

        public void Disable()
        {
            if (!Enabled) return;

            foreach (var function in Functions)
            {
                function.Disable();
            }

            Enabled = false;
            Log.Debug($"Disabled FunctionGroup for {Panel?.Name}");
        }

        public void SetOutputDevice(IOutputDevice outputDevice)
        {
            foreach (var function in Functions)
            {
                function.OutputDevice = outputDevice;
            }
        }

        public void SetInputDevice(IInputDevice inputDevice)
        {
            if (_inputDevice != null)
            {
                _inputDevice.Nrpn -= HandleNrpn;
                _inputDevice.ControlChange -= HandleControlChange;
            }

            inputDevice.Nrpn += HandleNrpn;
            inputDevice.ControlChange += HandleControlChange;
            _inputDevice = inputDevice;
        }

        private void HandleNrpn(NrpnMessage msg)
        {
            if (!Enabled) return;

            foreach (var function in Functions)
            {
                if (function.ControllerType == ControllerType.Nrpn)
                {
                    function.Handle(msg);
                }
            }
        }

        private void HandleControlChange(ControlChangeMessage msg)
        {
            if (!Enabled) return;

            foreach (var function in Functions)
            {
                if (function.ControllerType == ControllerType.ControlChange)
                {
                    function.Handle(msg);
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}