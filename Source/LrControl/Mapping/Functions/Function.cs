using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping.Functions
{
    public abstract class Function : INotifyPropertyChanged
    {
        private Controller _controller;
        private bool _enabled;
        private string _displayName;

        protected Function(LrApi api, string displayName)
        {
            Api = api;
            DisplayName = displayName;
        }

        protected LrApi Api { get; }

        public Controller Controller
        {
            get { return _controller; }
            set
            {
                if (Equals(value, _controller)) return;

                if (_controller != null)
                {
                    _controller.ControllerChanged -= OnControllerChanged;
                }

                _controller = value;
                _controller.ControllerChanged += OnControllerChanged;
                OnPropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Enable()
        {
            Enabled = true;
        }

        public virtual void Disable()
        {
            Enabled = false;
        }

        private void OnControllerChanged(int controllerValue)
        {
            if (!Enabled) return;

            ControllerChanged(controllerValue);
        }

        protected void ShowHud(string message)
        {
            if (Settings.ShowHudMessages)
            {
                Api.LrDialogs.ShowBezel(message, 0.25);
            }
        }

        protected abstract void ControllerChanged(int controllerValue);

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}