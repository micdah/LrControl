using System;
using System.ComponentModel;

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