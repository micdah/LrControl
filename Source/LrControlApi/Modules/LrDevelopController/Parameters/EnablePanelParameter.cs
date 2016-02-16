namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EnablePanelParameter : ParameterGroup
    {
        public readonly IParameter<bool> ToneCurve                        = new Parameter<bool>("EnableToneCurve", "Enable Tone Curve");
        public readonly IParameter<bool> ColorAdjustments                 = new Parameter<bool>("EnableColorAdjustments", "Enable Color Adjustments");
        public readonly IParameter<bool> SplitToning                      = new Parameter<bool>("EnableSplitToning", "Enable Split Toning");
        public readonly IParameter<bool> Detail                           = new Parameter<bool>("EnableDetail", "Enable Detail");
        public readonly IParameter<bool> LensCorrections                  = new Parameter<bool>("EnableLensCorrections", "Enable Lens Corrections");
        public readonly IParameter<bool> Effects                          = new Parameter<bool>("EnableEffects", "Enable Effects");
        public readonly IParameter<bool> Calibration                      = new Parameter<bool>("EnableCalibration", "Enable Camera Calibration");
        public readonly IParameter<bool> Retouch                          = new Parameter<bool>("EnableRetouch", "Enable Spot Removal");
        public readonly IParameter<bool> RedEye                           = new Parameter<bool>("EnableRedEye", "Enable Red Eye");
        public readonly IParameter<bool> GradientBasedCorrections         = new Parameter<bool>("EnableGradientBasedCorrections", "Enable Graduated Filter");
        public readonly IParameter<bool> CircularGradientBasedCorrections = new Parameter<bool>("EnableCircularGradientBasedCorrections", "Enable Radial Filter");
        public readonly IParameter<bool> PaintBasedCorrections            = new Parameter<bool>("EnablePaintBasedCorrections", "Enable Adjustment Brush");
        public readonly IParameter<bool> GrayscaleMix                     = new Parameter<bool>("EnableGrayscaleMix", "Enable Black & White Mix");

        internal EnablePanelParameter() : base("Toggle panels")
        {
            AddParameters(ToneCurve, ColorAdjustments, SplitToning, Detail, LensCorrections, Effects, Calibration,
                Retouch, RedEye, GradientBasedCorrections, CircularGradientBasedCorrections, PaintBasedCorrections,
                GrayscaleMix);
        }
    }
}