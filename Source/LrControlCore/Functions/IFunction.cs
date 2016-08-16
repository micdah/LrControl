using System;
using System.ComponentModel;
using LrControlCore.Device;

namespace LrControlCore.Functions
{
    public interface IFunction : INotifyPropertyChanged, IDisposable
    {
        string Key { get; }
        Controller Controller { get; set; }
        bool Enabled { get; }
        string DisplayName { get; set; }
        void Enable();
        void Disable();
    }
}