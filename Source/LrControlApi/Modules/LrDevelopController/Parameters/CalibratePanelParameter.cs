using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CalibratePanelParameter : Parameter<CalibratePanelParameter>
    {
        public static readonly IDevelopControllerParameter<ProfileValue> Profile         = new ProfileParameter("CameraProfile", "Profile");
        public static readonly IDevelopControllerParameter<int> ShadowTint               = new IntParameter("ShadowTint", "Shadows: Tint");
        public static readonly IDevelopControllerParameter<int> RedHue                   = new IntParameter("RedHue", "Red Primary: Hue");
        public static readonly IDevelopControllerParameter<int> RedSaturation            = new IntParameter("RedSaturation", "Red Primary: Saturation");
        public static readonly IDevelopControllerParameter<int> GreenHue                 = new IntParameter("GreenHue", "Green Primary: Hue");
        public static readonly IDevelopControllerParameter<int> GreenSaturation          = new IntParameter("GreenSaturation", "Green Primary: Saturation");
        public static readonly IDevelopControllerParameter<int> BlueHue                  = new IntParameter("BlueHue", "Blue Primary: Hue");
        public static readonly IDevelopControllerParameter<int> BlueSaturation           = new IntParameter("BlueSaturation", "Blue Primary: Saturation");

        static CalibratePanelParameter()
        {
            AddParameters(Profile, ShadowTint, RedHue, RedSaturation, GreenHue, GreenSaturation, BlueHue, BlueSaturation);
        }

        private CalibratePanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        public class ProfileValue : ClassEnum<string,ProfileValue>
        {
            public static readonly ProfileValue AdobeStandard   = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraClear     = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraDeep      = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraLandscape = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraLight     = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraNeutral   = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraPortrait  = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraStandard  = new ProfileValue("Adobe Standard");
            public static readonly ProfileValue CameraVivid     = new ProfileValue("Adobe Standard");

            static ProfileValue()
            {
                AddEnums(AdobeStandard, CameraClear, CameraDeep, CameraLandscape, CameraLight, CameraNeutral,
                    CameraPortrait, CameraStandard, CameraVivid);
            }

            private ProfileValue(string name) : base(name, name)
            {
            }
        }

        private class ProfileParameter : CalibratePanelParameter, IDevelopControllerParameter<ProfileValue>
        {
            public ProfileParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }

        private class IntParameter : CalibratePanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}