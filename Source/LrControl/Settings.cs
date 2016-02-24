using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;

namespace micdah.LrControl
{
    public class Settings : INotifyPropertyChanged
    {
        public static Settings Current = new Settings();

        private bool _showHudMessages;

        private Settings()
        {
            ShowHudMessages = true;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}