using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Mapping;
using LrControl.Core.Midi;
using LrControl.Core.Util;
using RtMidi.Core;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;
using Serilog;

namespace LrControl.Core
{
    public class LrControlApplication : ILrControlApplication
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<LrControlApplication>();
        private readonly LrApi _lrApi;
        private readonly Settings _settings;
        private readonly IFunctionCatalog _functionCatalog;
        private readonly Device _device;
        private readonly FunctionGroupManager _functionGroupManager;
        private readonly List<IMidiInputDeviceInfo> _inputDeviceInfos = new List<IMidiInputDeviceInfo>();
        private readonly List<IMidiOutputDeviceInfo> _outputDeviceInfos = new List<IMidiOutputDeviceInfo>();
        private InputDeviceDecorator _inputDevice;
        private IMidiOutputDevice _outputDevice;
        
        public static ILrControlApplication Create()
        {
            return new LrControlApplication();
        }

        private LrControlApplication()
        {
            _settings = Configurations.Settings.LoadOrDefault();
            _lrApi = new LrApi();
            _functionCatalog = Functions.Catalog.FunctionCatalog.DefaultCatalog(_settings, _lrApi);
            _device = new Device();
            _functionGroupManager = FunctionGroupManager.DefaultGroups(_lrApi, _functionCatalog, _device);

            // Hookup module listener
            _lrApi.LrApplicationView.ModuleChanged += _functionGroupManager.EnableModule;
            _lrApi.ConnectionStatus += (connected, version) => ConnectionStatus?.Invoke(connected, version);

            // Listen for Setting changes
            _settings.PropertyChanged += SettingsOnPropertyChanged;

            // Restore previously selected input/output devices
            RefreshAvailableDevices(false);
            SetInputDevice(_inputDeviceInfos.FirstOrDefault(x => x.Name == _settings.LastUsedInputDevice));
            SetOutputDevice(_outputDeviceInfos.FirstOrDefault(x => x.Name == _settings.LastUsedOutputDevice));

            // Load configuration
            LoadConfiguration();

            Log.Information("LrControl application started, running {Version}", Environment.Version);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ConnectionStatusHandler ConnectionStatus;
        
        public ISettings Settings => _settings;
        public FunctionGroupManager FunctionGroupManager => _functionGroupManager;
        public IFunctionCatalog FunctionCatalog => _functionCatalog;
        public IEnumerable<InputDeviceInfo> InputDevices => _inputDeviceInfos.Select(x => new InputDeviceInfo(x));
        public IEnumerable<OutputDeviceInfo> OutputDevices => _outputDeviceInfos.Select(x => new OutputDeviceInfo(x));

        public InputDeviceInfo InputDevice => _inputDevice != null ? new InputDeviceInfo(_inputDevice) : null;
        public OutputDeviceInfo OutputDevice => _outputDevice != null ? new OutputDeviceInfo(_outputDevice) : null;
        
        public void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = new MappingConfiguration
            {
                Controllers = _device.GetConfiguration(),
                Modules = FunctionGroupManager.GetConfiguration()
            };

            MappingConfiguration.Save(conf, file);
        }

        public void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = MappingConfiguration.Load(file);
            if (conf == null) return;

            _device.Load(conf.Controllers);
            _device.ResetAllControls();

            FunctionGroupManager.Load(conf.Modules);

            // Enable current module group
            if (_lrApi.LrApplicationView.GetCurrentModuleName(out var currentModule))
                FunctionGroupManager.EnableModule(currentModule);
        }

        public void Reset()
        {
            _device.Clear();
            _functionGroupManager.Reset();
        }

        public void RefreshAvailableDevices(bool restorePrevious = true)
        {
            // Update input devices
            var inputDeviceName = InputDevice?.Name;

            _inputDeviceInfos.Clear();
            _inputDeviceInfos.AddRange(MidiDeviceManager.Default.InputDevices);

            OnPropertyChanged(nameof(InputDevices));

            if (inputDeviceName != null && restorePrevious)
            {
                SetInputDevice(_inputDeviceInfos.FirstOrDefault(x => x.Name == inputDeviceName));
            }

            // Update output devices
            var outputDeviceName = OutputDevice?.Name;

            _outputDeviceInfos.Clear();
            _outputDeviceInfos.AddRange(MidiDeviceManager.Default.OutputDevices);

            OnPropertyChanged(nameof(OutputDevices));
            
            if (outputDeviceName != null && restorePrevious)
            {
                SetOutputDevice(_outputDeviceInfos.FirstOrDefault(x => x.Name == outputDeviceName));
            }
        }

        public void SetInputDevice(InputDeviceInfo inputDeviceInfo)
        {
            SetInputDevice(_inputDeviceInfos.FirstOrDefault(inputDeviceInfo.MatchThisFunc));
        }

        private void SetInputDevice(IMidiInputDeviceInfo inputDeviceInfo)
        {
            _inputDevice?.Dispose();

            var inputDevice = inputDeviceInfo?.CreateDevice();
            if (inputDevice != null)
            {
                _inputDevice = new InputDeviceDecorator(inputDevice, 1000 / _settings.ParameterUpdateFrequency);
                _device.InputDevice = _inputDevice;

                if (!_inputDevice.IsOpen)
                    _inputDevice.Open();
            }
            else
            {
                _inputDevice = null;
                _device.InputDevice = null;
            }

            OnPropertyChanged(nameof(InputDevice));
        }

        public void SetOutputDevice(OutputDeviceInfo outputDeviceInfo)
        {
            var outputDevice = _outputDeviceInfos.FirstOrDefault(outputDeviceInfo.MatchThisFunc);
            SetOutputDevice(outputDevice);
        }

        private void SetOutputDevice(IMidiOutputDeviceInfo outputDeviceInfo)
        {
            if (_outputDevice != null && _outputDevice.IsOpen)
                _outputDevice.Close();

            var outputDevice = outputDeviceInfo?.CreateDevice();
            if (outputDevice != null)
            {
                _outputDevice = outputDevice;
                _device.OutputDevice = _outputDevice;

                if (!_outputDevice.IsOpen)
                    _outputDevice.Open();
            }
            else
            {
                _outputDevice = null;
                _device.OutputDevice = null;
            }

            OnPropertyChanged(nameof(OutputDevice));
        }

        public string GetSettingsFolder()
        {
            return Path.GetDirectoryName(Serializer.ResolveRelativeFilename(MappingConfiguration.ConfigurationsFile));
        }

        public void UpdateConnectionStatus()
        {
            ConnectionStatus?.Invoke(_lrApi.Connected, _lrApi.ApiVersion);
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Settings.ParameterUpdateFrequency)) return;

            if (InputDevice != null)
            {
                _inputDevice.UpdateInterval = 1000 / _settings.ParameterUpdateFrequency;
            }
        }

        public void Dispose()
        {
            if (_settings.SaveConfigurationOnExit)
            {
                Log.Information("Saving configuration");
                SaveConfiguration();
            }

            Log.Debug("Saving last used input ({InputName}) and output ({OutputName}) devices", InputDevice?.Name,
                OutputDevice?.Name);
            _settings.SetLastUsed(_inputDevice, _outputDevice);

            Log.Information("Saving settings");
            _settings.Save();

            Log.Information("Closing API");
            _lrApi.Dispose();
            
            Serilog.Log.CloseAndFlush();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
