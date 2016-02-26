using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControl.Core;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Catalog;
using micdah.LrControlApi;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrApplicationView;
using Midi.Devices;
using Midi.Enums;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly LrApi _api;
        private readonly IInputDevice _inputDevice;
        private readonly IOutputDevice _outputDevice;

        // Faders
        private Controller _fader0 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 0, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader1 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 1, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader2 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 2, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader3 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 3, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader4 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 4, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader5 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 5, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader6 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 6, new Range(0, 1023)) { ControllerType = ControllerType.Fader };
        private Controller _fader7 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 7, new Range(0, 1023)) { ControllerType = ControllerType.Fader };

        // Encoders
        private Controller _encoder0 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn,  8, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder1 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn,  9, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder2 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 10, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder3 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 11, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder4 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 12, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder5 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 13, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder6 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 14, new Range(0, 511)) { ControllerType = ControllerType.Encoder };
        private Controller _encoder7 = new Controller(Channel.Channel1, ControllerMessageType.Nrpn, 15, new Range(0, 511)) { ControllerType = ControllerType.Encoder };

        // Buttons
        private Controller _button0  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  0, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button1  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  1, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button2  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  2, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button3  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  3, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button4  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  4, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button5  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  5, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button6  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  6, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button7  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  7, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button8  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  8, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button9  = new Controller(Channel.Channel1, ControllerMessageType.ControlChange,  9, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button10 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 10, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button11 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 11, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button12 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 12, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button13 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 13, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button14 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 14, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button15 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 15, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button16 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 16, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button17 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 17, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button18 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 18, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button19 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 19, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button20 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 20, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button21 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 21, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button22 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 22, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button23 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 23, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button24 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 24, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button25 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 25, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button26 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 26, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button27 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 27, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button28 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 28, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button29 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 29, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button30 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 30, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button31 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 31, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button32 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 32, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button33 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 33, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button34 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 34, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button35 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 35, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button36 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 36, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private Controller _button37 = new Controller(Channel.Channel1, ControllerMessageType.ControlChange, 37, new Range(0, 127)) { ControllerType = ControllerType.Button };
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);
            
            _api = new LrApi();
            _api.ConnectionStatus += UpdateConnectionStatus;

            _inputDevice = DeviceManager.InputDevices.Single(x => x.Name == "BCF2000");
            _inputDevice.Open();
            _inputDevice.StartReceiving(null);

            _outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            _outputDevice.Open();

            // View model
            ViewModel = new MainWindowModel
            {
                ControllerManager = new ControllerManager(new List<Controller>
                {
                    _fader0,_fader1,_fader2,_fader3,_fader4,_fader5,_fader6,_fader7,
                    _encoder0,_encoder1,_encoder2,_encoder3,_encoder4,_encoder5,_encoder6,_encoder7,
                    _button0,_button1,_button2,_button3,_button4,_button5,_button6,_button7,_button8,_button9,
                    _button10,_button11,_button12,_button13,_button14,_button15,_button16,_button17,_button18,_button19,
                    _button20,_button21,_button22,_button23,_button24,_button25,_button26,_button27,_button28,_button29,
                    _button30,_button31,_button32,_button33,_button34,_button35,_button36,_button37
                }),
                FunctionCatalog = FunctionCatalog.DefaultCatalog(_api),
                FunctionGroupCatalog = FunctionGroupCatalog.DefaultGroups(_api),
            };

            ViewModel.ControllerManager.SetInputDevice(_inputDevice);
            ViewModel.ControllerManager.SetOutputDevice(_outputDevice);
            ViewModel.ControllerManager.ResetAllControls();
            ViewModel.FunctionGroupCatalog.InitControllers(ViewModel.ControllerManager);

            // Enable module group matching currently active module
            Module currentModule;
            if (_api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                ModuleGroup.EnableGroupFor(currentModule);
            }

            // Automatically switch module group
            _api.LrApplicationView.ModuleChanged += ModuleGroup.EnableGroupFor;
        }

        public MainWindowModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (Equals(value, _viewModel)) return;
                _viewModel = value;
                OnPropertyChanged();
            }
        }
        private void UpdateConnectionStatus(bool connected, string apiVersion)
        {
            Dispatcher.InvokeAsync(() =>
            {
                Connected.Text = $"{(connected ? $"Connected ({apiVersion})" : "Not connected")}";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Current.Save();
        }
    }
}