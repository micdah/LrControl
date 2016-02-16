using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LensCorrectionsPanelParameter : Parameter<LensCorrectionsPanelParameter>, IDevelopControllerParameter
    {
        public static readonly LensCorrectionsPanelParameter LensProfileDistortionScale          = new LensCorrectionsPanelParameter("LensProfileDistortionScale", "Profile: Distortion Scale");
        public static readonly LensCorrectionsPanelParameter LensProfileVignettingScale          = new LensCorrectionsPanelParameter("LensProfileVignettingScale", "Profile: Vignetting Scale");
        public static readonly LensCorrectionsPanelParameter LensProfileChromaticAberrationScale = new LensCorrectionsPanelParameter("LensProfileChromaticAberrationScale", "Profile: Chromatic Aberration Scale");
        public static readonly LensCorrectionsPanelParameter DefringePurpleAmount                = new LensCorrectionsPanelParameter("DefringePurpleAmount", "Defringe: Purple Amount");
        public static readonly LensCorrectionsPanelParameter DefringePurpleHueLo                 = new LensCorrectionsPanelParameter("DefringePurpleHueLo", "Defringe: Purple Hue (Low)");
        public static readonly LensCorrectionsPanelParameter DefringePurpleHueHi                 = new LensCorrectionsPanelParameter("DefringePurpleHueHi", "Defringe: Purple Hue (High)");
        public static readonly LensCorrectionsPanelParameter DefringeGreenAmount                 = new LensCorrectionsPanelParameter("DefringeGreenAmount", "Defringe: Green Amount");
        public static readonly LensCorrectionsPanelParameter DefringeGreenHueLo                  = new LensCorrectionsPanelParameter("DefringeGreenHueLo", "Defringe: Green Hue (Low)");
        public static readonly LensCorrectionsPanelParameter DefringeGreenHueHi                  = new LensCorrectionsPanelParameter("DefringeGreenHueHi", "Defringe: Green Hue (High)");
        public static readonly LensCorrectionsPanelParameter LensManualDistortionAmount          = new LensCorrectionsPanelParameter("LensManualDistortionAmount", "Transform: Distortion");
        public static readonly LensCorrectionsPanelParameter PerspectiveVertical                 = new LensCorrectionsPanelParameter("PerspectiveVertical", "Transform: Vertical");
        public static readonly LensCorrectionsPanelParameter PerspectiveHorizontal               = new LensCorrectionsPanelParameter("PerspectiveHorizontal", "Transform: Horizontal");
        public static readonly LensCorrectionsPanelParameter PerspectiveRotate                   = new LensCorrectionsPanelParameter("PerspectiveRotate", "Transform: Rotate");
        public static readonly LensCorrectionsPanelParameter PerspectiveScale                    = new LensCorrectionsPanelParameter("PerspectiveScale", "Transform: Scale");
        public static readonly LensCorrectionsPanelParameter PerspectiveAspect                   = new LensCorrectionsPanelParameter("PerspectiveAspect", "Transform: Aspect");
        public static readonly LensCorrectionsPanelParameter PerspectiveUpright                  = new LensCorrectionsPanelParameter("PerspectiveUpright", "Upright", typeof(UprightValue));

        private LensCorrectionsPanelParameter(string value, string name, Type valueType) : base(name, value, valueType)
        {
        }

        private LensCorrectionsPanelParameter(string value, string name) : base(name, value, typeof(int))
        {
        }

        public class UprightValue : ClassEnum<int,UprightValue>
        {
            public static readonly UprightValue Off      = new UprightValue(0, "Off");
            public static readonly UprightValue Auto     = new UprightValue(1, "Auto");
            public static readonly UprightValue Full     = new UprightValue(2, "Full");
            public static readonly UprightValue Level    = new UprightValue(3, "Level");
            public static readonly UprightValue Vertical = new UprightValue(4, "Vertical");

            static UprightValue()
            {
                AddEnums(Off, Auto, Full, Level, Vertical);
            }

            private UprightValue(int value, string name) : base(value, name)
            {
            }
        }
    }
}