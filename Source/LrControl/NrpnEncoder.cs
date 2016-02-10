using Midi.Devices;
using Midi.Enums;

namespace micdah.LrControl
{
    internal class NrpnEncoder
    {
        private readonly OutputDevice _device;

        public NrpnEncoder(OutputDevice device)
        {
            _device = device;
        }

        public void SendNrpn(int parameter, int value)
        {
            var parameter14 = new Int14(parameter);
            var value14 = new Int14(value);

            _device.SendControlChange(Channel.Channel1, Control.NonRegisteredParameterMSB, parameter14.MSB);
            _device.SendControlChange(Channel.Channel1, Control.NonRegisteredParameterLSB, parameter14.LSB);
            _device.SendControlChange(Channel.Channel1, Control.DataEntryMSB, value14.MSB);
            _device.SendControlChange(Channel.Channel1, Control.DataEntryLSB, value14.LSB);
        }
    }
}