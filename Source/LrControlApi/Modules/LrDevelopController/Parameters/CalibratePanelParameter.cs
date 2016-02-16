namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CalibratePanelParameter : ParameterGroup
    {
        public readonly IParameter<ProfileValue> Profile         = new Parameter<ProfileValue>("CameraProfile", "Profile");
        public readonly IParameter<int> ShadowTint               = new Parameter<int>("ShadowTint", "Shadows: Tint");
        public readonly IParameter<int> RedHue                   = new Parameter<int>("RedHue", "Red Primary: Hue");
        public readonly IParameter<int> RedSaturation            = new Parameter<int>("RedSaturation", "Red Primary: Saturation");
        public readonly IParameter<int> GreenHue                 = new Parameter<int>("GreenHue", "Green Primary: Hue");
        public readonly IParameter<int> GreenSaturation          = new Parameter<int>("GreenSaturation", "Green Primary: Saturation");
        public readonly IParameter<int> BlueHue                  = new Parameter<int>("BlueHue", "Blue Primary: Hue");
        public readonly IParameter<int> BlueSaturation           = new Parameter<int>("BlueSaturation", "Blue Primary: Saturation");

        internal CalibratePanelParameter()
        {
            AddParameters(Profile, ShadowTint, RedHue, RedSaturation, GreenHue, GreenSaturation, BlueHue, BlueSaturation);
        }
    }
}