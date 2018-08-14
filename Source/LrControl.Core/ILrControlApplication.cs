using System;
using System.Collections.Generic;
using System.ComponentModel;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Mapping;

namespace LrControl.Core
{
    public interface ILrControlApplication : IDisposable
    {
        event ConnectionStatusHandler ConnectionStatus;
        ISettings Settings { get; }
        FunctionGroupManager FunctionGroupManager { get; }
        IFunctionCatalog FunctionCatalog { get; }
        IDeviceBrowser DeviceBrowser { get; }
        IDeviceManager DeviceManager { get; }

        void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void Reset();
        string GetSettingsFolder();
        void UpdateConnectionStatus();
    }
}