using System;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CalibratePanelParameter : ParameterGroup<CalibratePanelParameter>
    {
        public static readonly IParameter<ProfileValue> Profile         = new Parameter<ProfileValue>("CameraProfile", "Profile");
        public static readonly IParameter<int> ShadowTint               = new Parameter<int>("ShadowTint", "Shadows: Tint");
        public static readonly IParameter<int> RedHue                   = new Parameter<int>("RedHue", "Red Primary: Hue");
        public static readonly IParameter<int> RedSaturation            = new Parameter<int>("RedSaturation", "Red Primary: Saturation");
        public static readonly IParameter<int> GreenHue                 = new Parameter<int>("GreenHue", "Green Primary: Hue");
        public static readonly IParameter<int> GreenSaturation          = new Parameter<int>("GreenSaturation", "Green Primary: Saturation");
        public static readonly IParameter<int> BlueHue                  = new Parameter<int>("BlueHue", "Blue Primary: Hue");
        public static readonly IParameter<int> BlueSaturation           = new Parameter<int>("BlueSaturation", "Blue Primary: Saturation");

        static CalibratePanelParameter()
        {
            AddParameters(Profile, ShadowTint, RedHue, RedSaturation, GreenHue, GreenSaturation, BlueHue, BlueSaturation);
        }
    }
}