using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LensCorrectionsPanelParameter : Parameter<LensCorrectionsPanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> LensProfileDistortionScale                   = new IntParameter("LensProfileDistortionScale", "Profile: Distortion Scale");
        public static readonly IDevelopControllerParameter<int> LensProfileVignettingScale                   = new IntParameter("LensProfileVignettingScale", "Profile: Vignetting Scale");
        public static readonly IDevelopControllerParameter<int> LensProfileChromaticAberrationScale          = new IntParameter("LensProfileChromaticAberrationScale", "Profile: Chromatic Aberration Scale");
        public static readonly IDevelopControllerParameter<int> DefringePurpleAmount                         = new IntParameter("DefringePurpleAmount", "Defringe: Purple Amount");
        public static readonly IDevelopControllerParameter<int> DefringePurpleHueLo                          = new IntParameter("DefringePurpleHueLo", "Defringe: Purple Hue (Low)");
        public static readonly IDevelopControllerParameter<int> DefringePurpleHueHi                          = new IntParameter("DefringePurpleHueHi", "Defringe: Purple Hue (High)");
        public static readonly IDevelopControllerParameter<int> DefringeGreenAmount                          = new IntParameter("DefringeGreenAmount", "Defringe: Green Amount");
        public static readonly IDevelopControllerParameter<int> DefringeGreenHueLo                           = new IntParameter("DefringeGreenHueLo", "Defringe: Green Hue (Low)");
        public static readonly IDevelopControllerParameter<int> DefringeGreenHueHi                           = new IntParameter("DefringeGreenHueHi", "Defringe: Green Hue (High)");
        public static readonly IDevelopControllerParameter<int> LensManualDistortionAmount                   = new IntParameter("LensManualDistortionAmount", "Transform: Distortion");
        public static readonly IDevelopControllerParameter<int> PerspectiveVertical                          = new IntParameter("PerspectiveVertical", "Transform: Vertical");
        public static readonly IDevelopControllerParameter<int> PerspectiveHorizontal                        = new IntParameter("PerspectiveHorizontal", "Transform: Horizontal");
        public static readonly IDevelopControllerParameter<int> PerspectiveRotate                            = new IntParameter("PerspectiveRotate", "Transform: Rotate");
        public static readonly IDevelopControllerParameter<int> PerspectiveScale                             = new IntParameter("PerspectiveScale", "Transform: Scale");
        public static readonly IDevelopControllerParameter<int> PerspectiveAspect                            = new IntParameter("PerspectiveAspect", "Transform: Aspect");
        public static readonly IDevelopControllerParameter<UprightValue> PerspectiveUpright                  = new UprightParameter("PerspectiveUpright", "Upright");

        static LensCorrectionsPanelParameter()
        {
            AddParameters(LensProfileDistortionScale, LensProfileVignettingScale, LensProfileChromaticAberrationScale,
                DefringePurpleAmount, DefringePurpleHueLo, DefringePurpleHueHi, DefringeGreenAmount, DefringeGreenHueLo,
                DefringeGreenHueHi, LensManualDistortionAmount, PerspectiveVertical, PerspectiveHorizontal,
                PerspectiveRotate, PerspectiveScale, PerspectiveAspect, PerspectiveUpright);
        }

        private LensCorrectionsPanelParameter(string name, string displayName) : base(name, displayName)
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

        private class IntParameter : LensCorrectionsPanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
        
        private class UprightParameter : LensCorrectionsPanelParameter, IDevelopControllerParameter<UprightValue>
        {
            public UprightParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}