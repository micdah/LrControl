using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Mapping;
using LrControl.Core.Util;
using LrControl.Functions.Catalog;
using LrControl.LrPlugin.Api;
using Serilog;

[assembly:InternalsVisibleTo("LrControl.Tests")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LrControl.Core
{
    public interface ILrControlApplication : IDisposable
    {
        event ConnectionStatusHandler ConnectionStatus;
        ISettings Settings { get; }
        FunctionMappingManager FunctionMappingManager { get; }
        IFunctionCatalog FunctionCatalog { get; }
        IDeviceBrowser DeviceBrowser { get; }
        IDeviceManager DeviceManager { get; }

        void SaveConfiguration(string file = null);
        void LoadConfiguration(string file = null);
        void Reset();
        string GetSettingsFolder();
        void UpdateConnectionStatus();
    }

    public class LrControlApplication : ILrControlApplication
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<LrControlApplication>();
        private readonly LrApi _lrApi;
        private readonly Settings _settings;
        private readonly IFunctionCatalog _functionCatalog;
        private readonly DeviceBrowser _deviceBrowser;
        private readonly DeviceManager _deviceManager;
        private readonly FunctionMappingManager _functionMappingManager;

        public static ILrControlApplication Create()
        {
            return new LrControlApplication();
        }

        private LrControlApplication()
        {
            _settings = Configurations.Settings.LoadOrDefault();
            _lrApi = new LrApi();
            _functionCatalog = new FunctionCatalog(_settings, _lrApi);
            _deviceBrowser = new DeviceBrowser();
            _deviceManager = new DeviceManager(_settings);
            _functionMappingManager = FunctionMappingManager.Create(_lrApi, _functionCatalog, _deviceManager);

            // Hookup module listener
            _lrApi.LrApplicationView.ModuleChanged += _functionMappingManager.EnableModule;
            _lrApi.ConnectionStatus += (connected, version) => ConnectionStatus?.Invoke(connected, version);

            // Restore previously selected input/output devices
            _deviceBrowser.Refresh();
            _deviceManager.SetInputDevice(_deviceBrowser.InputDevices
                .FirstOrDefault(x => x.Name == _settings.LastUsedInputDevice));
            _deviceManager.SetOutputDevice(_deviceBrowser.OutputDevices
                .FirstOrDefault(x => x.Name == _settings.LastUsedOutputDevice));

            // SetConfiguration configuration
            LoadConfiguration();

            Log.Information("LrControl application started, running {Version}", Environment.Version);
        }

        public event ConnectionStatusHandler ConnectionStatus;

        public ISettings Settings => _settings;
        public FunctionMappingManager FunctionMappingManager => _functionMappingManager;
        public IFunctionCatalog FunctionCatalog => _functionCatalog;
        public IDeviceBrowser DeviceBrowser => _deviceBrowser;
        public IDeviceManager DeviceManager => _deviceManager;

        public void SaveConfiguration(string file = null)
        {
            var conf = new MappingConfiguration
            {
                Controllers = _deviceManager.GetConfiguration(),
                Modules = FunctionMappingManager.GetConfiguration()
            };

            MappingConfiguration.Save(conf, file ?? MappingConfiguration.ConfigurationsFile);
        }

        public void LoadConfiguration(string file = null)
        {
            var conf = MappingConfiguration.Load(file ?? MappingConfiguration.ConfigurationsFile);
            if (conf == null) return;

            _deviceManager.SetConfiguration(conf.Controllers);
            FunctionMappingManager.Load(conf.Modules);

            // Enable current module group
            if (_lrApi.LrApplicationView.GetCurrentModuleName(out var currentModule))
                FunctionMappingManager.EnableModule(currentModule);
        }

        public void Reset()
        {
            _deviceManager.ClearControllers();
            _functionMappingManager.Reset();
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

            Log.Debug("Saving last used input ({InputName}) and output ({OutputName}) devices",
                _deviceManager.InputDevice?.Name,
                _deviceManager.OutputDevice?.Name);
            _settings.SetLastUsed(_deviceManager.InputDevice, _deviceManager.OutputDevice);

            Log.Information("Saving settings");
            _settings.Save();

            Log.Debug("Disposing {Name}", nameof(FunctionMappingManager));
            _functionMappingManager.Dispose();

            Log.Information("Closing API");
            _lrApi.Dispose();

            Serilog.Log.CloseAndFlush();
        }
    }
}