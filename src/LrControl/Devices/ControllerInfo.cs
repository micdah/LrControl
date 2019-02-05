using LrControl.LrPlugin.Api.Common;

namespace LrControl.Devices
{
    public class ControllerInfo
    {
        public ControllerId ControllerId { get; }
        public Range Range { get; }
        public int LastValue { get; private set; }

        public ControllerInfo(in ControllerId controllerId, Range initialRange)
        {
            ControllerId = controllerId;
            Range = initialRange;
        }

        public void Update(int value)
        {
            // Update range based on value
            if (value < Range)
                Range.Minimum = value;
            else if (value > Range)
                Range.Maximum = value;

            LastValue = value;
        }
    }
}