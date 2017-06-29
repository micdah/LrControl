using System;
using System.ComponentModel;
using LrControl.Core.Device;

namespace LrControl.Core.Functions
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