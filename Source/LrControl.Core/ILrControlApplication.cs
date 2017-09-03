using System;
using System.Collections.Generic;
using System.ComponentModel;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Mapping;
using Midi.Devices;

namespace LrControl.Core
{
    public interface ILrControlApplication : IDisposable, INotifyPropertyChanged
    {
        event ConnectionStatusHandler ConnectionStatus;
        ISettings Settings { get; }
        FunctionGroupManager FunctionGroupManager { get; }
        IFunctionCatalog FunctionCatalog { get; }
        IEnumerable<IInputDevice> InputDevices { get; }
        IEnumerable<IOutputDevice> OutputDevices { get; }
        IInputDevice InputDevice { get; }
        IOutputDevice OutputDevice { get; }

        void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile);
        void Reset();
        void RefreshAvailableDevices(bool restorePrevious = true);
        string GetSettingsFolder();
        void UpdateConnectionStatus();
        void SetInputDevice(IInputDevice inputDevice);
        void SetOutputDevice(IOutputDevice outputDevice);
    }
}