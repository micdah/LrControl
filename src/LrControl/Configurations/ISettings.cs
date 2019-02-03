using System.ComponentModel;

namespace LrControl.Configurations
{
    public interface ISettings : INotifyPropertyChanged
    {
        bool ShowHudMessages { get; set; }
        bool StartMinimized { get; set; }
        bool SaveConfigurationOnExit { get; set; }
        int ParameterUpdateFrequency { get; set; }
        string LastUsedInputDevice { get; }
        string LastUsedOutputDevice { get; }
    }
}