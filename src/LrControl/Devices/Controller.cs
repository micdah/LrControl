using LrControl.LrPlugin.Api.Common;

namespace LrControl.Devices
{
    public class Controller
    {
        private readonly ProfileManager _profileManager;
        public ControllerId ControllerId { get; }
        public Range Range { get; }
        public int LastValue { get; private set; }

        public Controller(in ControllerId controllerId, Range initialRange, ProfileManager profileManager)
        {
            _profileManager = profileManager;
            ControllerId = controllerId;
            Range = initialRange;
        }

        public void OnControllerInput(int value)
        {
            // Update range based on value
            if (value < Range)
                Range.Minimum = value;
            else if (value > Range)
                Range.Maximum = value;

            LastValue = value;
            
            _profileManager.OnControllerInput(ControllerId, Range, value);
        }
    }
}