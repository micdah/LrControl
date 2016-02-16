using System;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LensCorrectionsPanelParameter : ParameterGroup<LensCorrectionsPanelParameter>
    {
        public static readonly IParameter<int> LensProfileDistortionScale                   = new Parameter<int>("LensProfileDistortionScale", "Profile: Distortion Scale");
        public static readonly IParameter<int> LensProfileVignettingScale                   = new Parameter<int>("LensProfileVignettingScale", "Profile: Vignetting Scale");
        public static readonly IParameter<int> LensProfileChromaticAberrationScale          = new Parameter<int>("LensProfileChromaticAberrationScale", "Profile: Chromatic Aberration Scale");
        public static readonly IParameter<int> DefringePurpleAmount                         = new Parameter<int>("DefringePurpleAmount", "Defringe: Purple Amount");
        public static readonly IParameter<int> DefringePurpleHueLo                          = new Parameter<int>("DefringePurpleHueLo", "Defringe: Purple Hue (Low)");
        public static readonly IParameter<int> DefringePurpleHueHi                          = new Parameter<int>("DefringePurpleHueHi", "Defringe: Purple Hue (High)");
        public static readonly IParameter<int> DefringeGreenAmount                          = new Parameter<int>("DefringeGreenAmount", "Defringe: Green Amount");
        public static readonly IParameter<int> DefringeGreenHueLo                           = new Parameter<int>("DefringeGreenHueLo", "Defringe: Green Hue (Low)");
        public static readonly IParameter<int> DefringeGreenHueHi                           = new Parameter<int>("DefringeGreenHueHi", "Defringe: Green Hue (High)");
        public static readonly IParameter<int> LensManualDistortionAmount                   = new Parameter<int>("LensManualDistortionAmount", "Transform: Distortion");
        public static readonly IParameter<int> PerspectiveVertical                          = new Parameter<int>("PerspectiveVertical", "Transform: Vertical");
        public static readonly IParameter<int> PerspectiveHorizontal                        = new Parameter<int>("PerspectiveHorizontal", "Transform: Horizontal");
        public static readonly IParameter<int> PerspectiveRotate                            = new Parameter<int>("PerspectiveRotate", "Transform: Rotate");
        public static readonly IParameter<int> PerspectiveScale                             = new Parameter<int>("PerspectiveScale", "Transform: Scale");
        public static readonly IParameter<int> PerspectiveAspect                            = new Parameter<int>("PerspectiveAspect", "Transform: Aspect");
        public static readonly IParameter<UprightValue> PerspectiveUpright                  = new Parameter<UprightValue>("PerspectiveUpright", "Upright");

        static LensCorrectionsPanelParameter()
        {
            AddParameters(LensProfileDistortionScale, LensProfileVignettingScale, LensProfileChromaticAberrationScale,
                DefringePurpleAmount, DefringePurpleHueLo, DefringePurpleHueHi, DefringeGreenAmount, DefringeGreenHueLo,
                DefringeGreenHueHi, LensManualDistortionAmount, PerspectiveVertical, PerspectiveHorizontal,
                PerspectiveRotate, PerspectiveScale, PerspectiveAspect, PerspectiveUpright);
        }
    }
}