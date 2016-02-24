using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControl.Core;

namespace micdah.LrControl.Configurations
{
    public class Settings : INotifyPropertyChanged
    {
        private const string SettingsFile = "Settings.xml";

        public static readonly Settings Current;

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
                    StartMinimized = false
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}