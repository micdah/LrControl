using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using micdah.LrControl.Annotations;
using micdah.LrControl.Core;

namespace micdah.LrControl.Configurations
{
    public class Settings : INotifyPropertyChanged
    {
        private const string SettingsFile = @"..\Settings\Settings.xml";

        public static readonly Settings Current;

        private bool _showHudMessages;
        private bool _startMinimized;
        private bool _autoSaveOnClose;
        private int _parameterUpdateFrequency;
        private string _lastUsedInputDevice;
        private string _lastUsedOutputDevice;

        static Settings()
        {
            if (!Serializer.Load(SettingsFile, out Current))
            {
                // No saved settings, load defaults
                Current = new Settings
                {
                    ShowHudMessages = true,
                    StartMinimized = false,
                    AutoSaveOnClose = false,
                    ParameterUpdateFrequency = 30,
                };
            }
        }

        public void Save()
        {
            Serializer.Save(SettingsFile, this);
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

        public bool AutoSaveOnClose
        {
            get { return _autoSaveOnClose; }
            set
            {
                if (value == _autoSaveOnClose) return;
                _autoSaveOnClose = value;
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

        public void SetLastUsedFrom(MainWindowModel viewModel)
        {
            if (viewModel.InputDevice != null)
            {
                LastUsedInputDevice = viewModel.InputDevice.Name;
            }

            if (viewModel.OutputDevice != null)
            {
                LastUsedOutputDevice = viewModel.OutputDevice.Name;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}