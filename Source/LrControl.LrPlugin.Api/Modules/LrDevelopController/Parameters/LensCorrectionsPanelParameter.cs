namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public class LensCorrectionsPanelParameter : ParameterGroup
    {
        public readonly IParameter<int> LensProfileDistortionScale                      = new Parameter<int>("LensProfileDistortionScale", "Profile: Distortion Scale");
        public readonly IParameter<int> LensProfileVignettingScale                      = new Parameter<int>("LensProfileVignettingScale", "Profile: Vignetting Scale");
        public readonly IParameter<int> LensProfileChromaticAberrationScale             = new Parameter<int>("LensProfileChromaticAberrationScale", "Profile: Chromatic Aberration Scale");
        public readonly IParameter<int> DefringePurpleAmount                            = new Parameter<int>("DefringePurpleAmount", "Defringe: Purple Amount");
        public readonly IParameter<int> DefringePurpleHueLo                             = new Parameter<int>("DefringePurpleHueLo", "Defringe: Purple Hue (Low)");
        public readonly IParameter<int> DefringePurpleHueHi                             = new Parameter<int>("DefringePurpleHueHi", "Defringe: Purple Hue (High)");
        public readonly IParameter<int> DefringeGreenAmount                             = new Parameter<int>("DefringeGreenAmount", "Defringe: Green Amount");
        public readonly IParameter<int> DefringeGreenHueLo                              = new Parameter<int>("DefringeGreenHueLo", "Defringe: Green Hue (Low)");
        public readonly IParameter<int> DefringeGreenHueHi                              = new Parameter<int>("DefringeGreenHueHi", "Defringe: Green Hue (High)");
        public readonly IParameter<int> LensManualDistortionAmount                      = new Parameter<int>("LensManualDistortionAmount", "Transform: Distortion");
        public readonly IParameter<int> PerspectiveVertical                             = new Parameter<int>("PerspectiveVertical", "Transform: Vertical");
        public readonly IParameter<int> PerspectiveHorizontal                           = new Parameter<int>("PerspectiveHorizontal", "Transform: Horizontal");
        public readonly IParameter<double> PerspectiveRotate                            = new Parameter<double>("PerspectiveRotate", "Transform: Rotate");
        public readonly IParameter<int> PerspectiveScale                                = new Parameter<int>("PerspectiveScale", "Transform: Scale");
        public readonly IParameter<int> PerspectiveAspect                               = new Parameter<int>("PerspectiveAspect", "Transform: Aspect");
        public readonly IParameter<UprightValue> PerspectiveUpright                     = new Parameter<UprightValue>("PerspectiveUpright", "Upright");

        internal LensCorrectionsPanelParameter() : base("Lens Corrections")
        {
            AddParameters(LensProfileDistortionScale, LensProfileVignettingScale, LensProfileChromaticAberrationScale,
                DefringePurpleAmount, DefringePurpleHueLo, DefringePurpleHueHi, DefringeGreenAmount, DefringeGreenHueLo,
                DefringeGreenHueHi, LensManualDistortionAmount, PerspectiveVertical, PerspectiveHorizontal,
                PerspectiveRotate, PerspectiveScale, PerspectiveAspect, PerspectiveUpright);
        }
    }
}