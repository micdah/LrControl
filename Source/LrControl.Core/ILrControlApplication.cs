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
    public interface ILrControlApplication : IDisposable, INotifyPropertyChanged
    {
        event ConnectionStatusHandler ConnectionStatus;
        ISettings Settings { get; }
        FunctionGroupManager FunctionGroupManager { get; }
        IFunctionCatalog FunctionCatalog { get; }
        IEnumerable<InputDeviceInfo> InputDevices { get; }
        IEnumerable<OutputDeviceInfo> OutputDevices { get; }
        InputDeviceInfo InputDevice { get; }
        OutputDeviceInfo OutputDevice { get; }

        void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void Reset();
        void RefreshAvailableDevices(bool restorePrevious = true);
        string GetSettingsFolder();
        void UpdateConnectionStatus();
        void SetInputDevice(InputDeviceInfo inputDevice);
        void SetOutputDevice(OutputDeviceInfo outputDevice);
    }
}