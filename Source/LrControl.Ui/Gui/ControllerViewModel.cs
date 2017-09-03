using System.ComponentModel;
using System.Windows.Threading;
using LrControl.Api.Common;
using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;
using LrControl.Core.Mapping;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class ControllerViewModel : ViewModel
    {
        private readonly Controller _controller;
        private Range _range;
        private int _lastValue;
        private ControllerType _controllerType;
        private string _channel;
        private string _messageType;
        private int _controlNumber;

        public ControllerViewModel(Dispatcher dispatcher, Controller controller) : base(dispatcher)
        {
            _controller = controller;
            Range = _controller.Range;
            LastValue = _controller.LastValue;
            ControllerType = _controller.ControllerType;
            Channel = $"C{(int)_controller.Channel}";
            MessageType = _controller.MessageType == ControllerMessageType.Nrpn ? "NRPN" : "CC";
            ControlNumber = _controller.ControlNumber;

            _controller.PropertyChanged += ControllerOnPropertyChanged;
        }

        public Range Range
        {
            get => _range;
            private set
            {
                if (Equals(value, _range)) return;
                _range = value;
                OnPropertyChanged();
            }
        }

        public int LastValue
        {
            get => _lastValue;
            private set
            {
                if (value == _lastValue) return;
                _lastValue = value;
                OnPropertyChanged();
            }
        }

        public ControllerType ControllerType
        {
            get => _controllerType;
            private set
            {
                if (value == _controllerType) return;
                _controllerType = value;
                OnPropertyChanged();
            }
        }

        public string Channel
        {
            get => _channel;
            private set
            {
                if (value == _channel) return;
                _channel = value;
                OnPropertyChanged();
            }
        }

        public string MessageType
        {
            get => _messageType;
            private set
            {
                if (value == _messageType) return;
                _messageType = value;
                OnPropertyChanged();
            }
        }

        public int ControlNumber
        {
            get => _controlNumber;
            private set
            {
                if (value == _controlNumber) return;
                _controlNumber = value;
                OnPropertyChanged();
            }
        }

        public bool CanAssignFunction(ModuleGroup moduleGroup, bool functionGroupIsGlobal)
        {
            return moduleGroup.CanAssignFunction(_controller, functionGroupIsGlobal);
        }

        private void ControllerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is Controller controller)) return;

            SafeInvoke(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Controller.Range):
                        Range = controller.Range;
                        break;
                    case nameof(Controller.LastValue):
                        LastValue = controller.LastValue;
                        break;
                    case nameof(Controller.ControllerType):
                        ControllerType = controller.ControllerType;
                        break;
                    case nameof(Controller.Channel):
                        Channel = $"C{(int)controller.Channel}";
                        break;
                    case nameof(Controller.MessageType):
                        MessageType = controller.MessageType == ControllerMessageType.Nrpn ? "NRPN" : "CC";
                        break;
                    case nameof(Controller.ControlNumber):
                        ControlNumber = controller.ControlNumber;
                        break;
                }
            });
        }

        protected override void Disposing()
        {
            _controller.PropertyChanged -= ControllerOnPropertyChanged;
        }
    }
}