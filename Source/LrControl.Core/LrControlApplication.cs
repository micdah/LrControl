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
using LrControl.Core.Util;
using RtMidi.Core;
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
        private readonly DeviceManager _deviceManager;
        private readonly FunctionGroupManager _functionGroupManager;
        private readonly List<IMidiInputDeviceInfo> _inputDeviceInfos = new List<IMidiInputDeviceInfo>();
        private readonly List<IMidiOutputDeviceInfo> _outputDeviceInfos = new List<IMidiOutputDeviceInfo>();
        
        public static ILrControlApplication Create()
        {
            return new LrControlApplication();
        }

        private LrControlApplication()
        {
            _settings = Configurations.Settings.LoadOrDefault();
            _lrApi = new LrApi();
            _functionCatalog = Functions.Catalog.FunctionCatalog.CreateCatalog(_settings, _lrApi);
            _deviceManager = new DeviceManager(_settings);
            _functionGroupManager = FunctionGroupManager.DefaultGroups(_lrApi, _functionCatalog, _deviceManager);

            // Hookup module listener
            _lrApi.LrApplicationView.ModuleChanged += _functionGroupManager.EnableModule;
            _lrApi.ConnectionStatus += (connected, version) => ConnectionStatus?.Invoke(connected, version);

            // Restore previously selected input/output devices
            RefreshAvailableDevices(false);
            SetInputDevice(InputDevices.FirstOrDefault(x => x.Name == _settings.LastUsedInputDevice));
            SetOutputDevice(OutputDevices.FirstOrDefault(x => x.Name == _settings.LastUsedOutputDevice));

            // SetConfiguration configuration
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

        // TODO Remove
        public InputDeviceInfo InputDevice => _deviceManager.InputDevice;
        
        // TODO Remove
        public OutputDeviceInfo OutputDevice => _deviceManager.OutputDevice;
        
        public void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = new MappingConfiguration
            {
                Controllers = _deviceManager.GetConfiguration(),
                Modules = FunctionGroupManager.GetConfiguration()
            };

            MappingConfiguration.Save(conf, file);
        }

        public void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = MappingConfiguration.Load(file);
            if (conf == null) return;

            _deviceManager.SetConfiguration(conf.Controllers);
            FunctionGroupManager.Load(conf.Modules);

            // Enable current module group
            if (_lrApi.LrApplicationView.GetCurrentModuleName(out var currentModule))
                FunctionGroupManager.EnableModule(currentModule);
        }

        public void Reset()
        {
            _deviceManager.Clear();
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
                SetInputDevice(InputDevices.FirstOrDefault(x => x.Name == inputDeviceName));
            }

            // Update output devices
            var outputDeviceName = OutputDevice?.Name;

            _outputDeviceInfos.Clear();
            _outputDeviceInfos.AddRange(MidiDeviceManager.Default.OutputDevices);

            OnPropertyChanged(nameof(OutputDevices));
            
            if (outputDeviceName != null && restorePrevious)
            {
                SetOutputDevice(OutputDevices.FirstOrDefault(x => x.Name == outputDeviceName));
            }
        }

        // TODO Remove in favor of IDeviceManager property
        public void SetInputDevice(InputDeviceInfo inputDeviceInfo)
        {
            _deviceManager.SetInputDevice(inputDeviceInfo);
        }
        
        // TODO Remove in favor of IDeviceManager property
        public void SetOutputDevice(OutputDeviceInfo outputDeviceInfo)
        {
            _deviceManager.SetOutputDevice(outputDeviceInfo);
        }

        public string GetSettingsFolder()
        {
            return Path.GetDirectoryName(Serializer.ResolveRelativeFilename(MappingConfiguration.ConfigurationsFile));
        }

        public void UpdateConnectionStatus()
        {
            ConnectionStatus?.Invoke(_lrApi.Connected, _lrApi.ApiVersion);
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
            _settings.SetLastUsed(_deviceManager.InputDevice, _deviceManager.OutputDevice);

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
