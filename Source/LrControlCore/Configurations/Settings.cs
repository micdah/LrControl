using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Util;
using Midi.Devices;

namespace LrControlCore.Configurations
{
    public class Settings : INotifyPropertyChanged
    {
        private const string SettingsFile = @"..\Settings\Settings.xml";

        public static readonly Settings Current;
        private string _lastUsedInputDevice;
        private string _lastUsedOutputDevice;
        private int _parameterUpdateFrequency;
        private bool _saveConfigurationOnExit;

        private bool _showHudMessages;
        private bool _startMinimized;

        static Settings()
        {
            if (!Serializer.Load(SettingsFile, out Current))
            {
                // No saved settings, load defaults
                Current = new Settings
                {
                    ShowHudMessages = true,
                    StartMinimized = false,
                    SaveConfigurationOnExit = false,
                    ParameterUpdateFrequency = 30
                };
            }
        }

        public bool ShowHudMessages
        {
            get { return _showHudMessages; }
            set
            {
                if (value == _showHudMessages) return;
                _showHudMessages = value;
                OnPropertyChanged();
            }
        }

        public bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                if (value == _startMinimized) return;
                _startMinimized = value;
                OnPropertyChanged();
            }
        }

        public bool SaveConfigurationOnExit
        {
            get { return _saveConfigurationOnExit; }
            set
            {
                if (value == _saveConfigurationOnExit) return;
                _saveConfigurationOnExit = value;
                OnPropertyChanged();
            }
        }

        public int ParameterUpdateFrequency
        {
            get { return _parameterUpdateFrequency; }
            set
            {
                if (value == _parameterUpdateFrequency) return;
                _parameterUpdateFrequency = value;
                OnPropertyChanged();
            }
        }

        public string LastUsedInputDevice
        {
            get { return _lastUsedInputDevice; }
            set
            {
                if (value == _lastUsedInputDevice) return;
                _lastUsedInputDevice = value;
                OnPropertyChanged();
            }
        }

        public string LastUsedOutputDevice
        {
            get { return _lastUsedOutputDevice; }
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