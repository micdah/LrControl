﻿using System;
using System.IO;
using System.Linq;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Mapping;
using LrControl.Core.Util;
using Serilog;

namespace LrControl.Core
{
    public class LrControlApplication : ILrControlApplication
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<LrControlApplication>();
        private readonly LrApi _lrApi;
        private readonly Settings _settings;
        private readonly IFunctionCatalog _functionCatalog;
        private readonly DeviceBrowser _deviceBrowser;
        private readonly DeviceManager _deviceManager;
        private readonly FunctionGroupManager _functionGroupManager;

        public static ILrControlApplication Create()
        {
            return new LrControlApplication();
        }

        private LrControlApplication()
        {
            _settings = Configurations.Settings.LoadOrDefault();
            _lrApi = new LrApi();
            _functionCatalog = Functions.Catalog.FunctionCatalog.CreateCatalog(_settings, _lrApi);
            _deviceBrowser = new DeviceBrowser();
            _deviceManager = new DeviceManager(_settings);
            _functionGroupManager = FunctionGroupManager.DefaultGroups(_lrApi, _functionCatalog, _deviceManager);

            // Hookup module listener
            _lrApi.LrApplicationView.ModuleChanged += _functionGroupManager.EnableModule;
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
        public FunctionGroupManager FunctionGroupManager => _functionGroupManager;
        public IFunctionCatalog FunctionCatalog => _functionCatalog;
        public IDeviceBrowser DeviceBrowser => _deviceBrowser;
        public IDeviceManager DeviceManager => _deviceManager;

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

            Log.Information("Closing API");
            _lrApi.Dispose();

            Serilog.Log.CloseAndFlush();
        }
    }
}