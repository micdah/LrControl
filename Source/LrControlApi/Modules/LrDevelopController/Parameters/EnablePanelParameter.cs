using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EnablePanelParameter : ParameterGroup<EnablePanelParameter>
    {
        public static readonly IParameter<bool> ToneCurve                        = new Parameter<bool>("EnableToneCurve", "Enable Tone Curve");
        public static readonly IParameter<bool> ColorAdjustments                 = new Parameter<bool>("EnableColorAdjustments", "Enable Color Adjustments");
        public static readonly IParameter<bool> SplitToning                      = new Parameter<bool>("EnableSplitToning", "Enable Split Toning");
        public static readonly IParameter<bool> Detail                           = new Parameter<bool>("EnableDetail", "Enable Detail");
        public static readonly IParameter<bool> LensCorrections                  = new Parameter<bool>("EnableLensCorrections", "Enable Lens Corrections");
        public static readonly IParameter<bool> Effects                          = new Parameter<bool>("EnableEffects", "Enable Effects");
        public static readonly IParameter<bool> Calibration                      = new Parameter<bool>("EnableCalibration", "Enable Camera Calibration");
        public static readonly IParameter<bool> Retouch                          = new Parameter<bool>("EnableRetouch", "Enable Spot Removal");
        public static readonly IParameter<bool> RedEye                           = new Parameter<bool>("EnableRedEye", "Enable Red Eye");
        public static readonly IParameter<bool> GradientBasedCorrections         = new Parameter<bool>("EnableGradientBasedCorrections", "Enable Graduated Filter");
        public static readonly IParameter<bool> CircularGradientBasedCorrections = new Parameter<bool>("EnableCircularGradientBasedCorrections", "Enable Radial Filter");
        public static readonly IParameter<bool> PaintBasedCorrections            = new Parameter<bool>("EnablePaintBasedCorrections", "Enable Adjustment Brush");
        public static readonly IParameter<bool> GrayscaleMix                     = new Parameter<bool>("EnableGrayscaleMix", "Enable Black & White Mix");

        static EnablePanelParameter()
        {
            AddParameters(ToneCurve, ColorAdjustments, SplitToning, Detail, LensCorrections, Effects, Calibration,
                Retouch, RedEye, GradientBasedCorrections, CircularGradientBasedCorrections, PaintBasedCorrections,
                GrayscaleMix);
        }
    }
}