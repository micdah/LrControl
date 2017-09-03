using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Core.Util;
using Midi.Devices;

namespace LrControl.Core.Configurations
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

    public class Settings : ISettings
    {
        private const string SettingsFile = @"..\Settings\Settings.xml";

        private string _lastUsedInputDevice;
        private string _lastUsedOutputDevice;
        private int _parameterUpdateFrequency;
        private bool _saveConfigurationOnExit;

        private bool _showHudMessages;
        private bool _startMinimized;

        internal static Settings LoadOrDefault()
        {
            // Load existing
            if (Serializer.Load(SettingsFile, out Settings settings))
                return settings;

            // No existing, return default settings
            return new Settings
            {
                ShowHudMessages = true,
                StartMinimized = false,
                SaveConfigurationOnExit = false,
                ParameterUpdateFrequency = 30
            };
        }

        private Settings()
        {
        }

        public bool ShowHudMessages
        {
            get => _showHudMessages;
            set
            {
                if (value == _showHudMessages) return;
                _showHudMessages = value;
                OnPropertyChanged();
            }
        }

        public bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                if (value == _startMinimized) return;
                _startMinimized = value;
                OnPropertyChanged();
            }
        }

        public bool SaveConfigurationOnExit
        {
            get => _saveConfigurationOnExit;
            set
            {
                if (value == _saveConfigurationOnExit) return;
                _saveConfigurationOnExit = value;
                OnPropertyChanged();
            }
        }

        public int ParameterUpdateFrequency
        {
            get => _parameterUpdateFrequency;
            set
            {
                if (value == _parameterUpdateFrequency) return;
                _parameterUpdateFrequency = value;
                OnPropertyChanged();
            }
        }

        public string LastUsedInputDevice
        {
            get => _lastUsedInputDevice;
            set
            {
                if (value == _lastUsedInputDevice) return;
                _lastUsedInputDevice = value;
                OnPropertyChanged();
            }
        }

        public string LastUsedOutputDevice
        {
            get => _lastUsedOutputDevice;
            set
            {
                if (value == _lastUsedOutputDevice) return;
                _lastUsedOutputDevice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            Serializer.Save(SettingsFile, this);
        }

        public void SetLastUsed(IInputDevice inputDevice, IOutputDevice outputDevice)
        {
            if (inputDevice != null)
            {
                LastUsedInputDevice = inputDevice.Name;
            }

            if (outputDevice != null)
            {
                LastUsedOutputDevice = outputDevice.Name;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}