using System;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl
{
    internal class NrpnDecocer : IDisposable
    {
        public delegate void NrpnHandler(int parameter, int value);

        private readonly InputDevice _device;
        private bool _disposed;

        private Control? _lastControl;
        private Int14 _parameter;
        private Int14 _value;
        
        public NrpnDecocer(InputDevice device)
        {
            _device = device;

            _device.ControlChange += DeviceOnControlChange;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _device.ControlChange -= DeviceOnControlChange;
                _disposed = true;
            }
        }

        public event NrpnHandler OnNrpn;

        private void DeviceOnControlChange(ControlChangeMessage msg)
        {
            //(BCF2000) ControlChange: Channel=0 Control=99 Value=0
            //(BCF2000) ControlChange: Channel=0 Control=98 Value=0
            //(BCF2000) ControlChange: Channel=0 Control=6 Value=0
            //(BCF2000) ControlChange: Channel=0 Control=38 Value=4

            if (IsExpectedControl(msg.Control))
            {
                SaveValue(msg.Control, msg.Value);

                if (msg.Control == Control.DataEntryLSB)
                {
                    var parameter = _parameter.Value;
                    var value = _value.Value;
                    OnOnNrpn(parameter, value);
                }
            }


            _lastControl = msg.Control;
        }

        private bool IsExpectedControl(Control control)
        {
            switch (control)
            {
                case Control.NonRegisteredParameterLSB:
                    return _lastControl == Control.NonRegisteredParameterMSB;
                case Control.DataEntryMSB:
                    return _lastControl == Control.NonRegisteredParameterLSB;
                case Control.DataEntryLSB:
                    return _lastControl == Control.DataEntryMSB;
            }
            return true;
        }

        private void SaveValue(Control control, int value)
        {
            switch (control)
            {
                case Control.NonRegisteredParameterMSB:
                    _parameter.MSB = value;
                    break;
                case Control.NonRegisteredParameterLSB:
                    _parameter.LSB = value;
                    break;
                case Control.DataEntryMSB:
                    _value.MSB = value;
                    break;
                case Control.DataEntryLSB:
                    _value.LSB = value;
                    break;
            }
        }
        
        protected virtual void OnOnNrpn(int parameter, int value)
        {
            OnNrpn?.Invoke(parameter, value);
        }
    }
}