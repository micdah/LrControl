using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EnablePanelParameter : Parameter<EnablePanelParameter>
    {
        public static readonly IDevelopControllerParameter<bool> ToneCurve                        = new BoolParameter("EnableToneCurve", "Enable Tone Curve");
        public static readonly IDevelopControllerParameter<bool> ColorAdjustments                 = new BoolParameter("EnableColorAdjustments", "Enable Color Adjustments");
        public static readonly IDevelopControllerParameter<bool> SplitToning                      = new BoolParameter("EnableSplitToning", "Enable Split Toning");
        public static readonly IDevelopControllerParameter<bool> Detail                           = new BoolParameter("EnableDetail", "Enable Detail");
        public static readonly IDevelopControllerParameter<bool> LensCorrections                  = new BoolParameter("EnableLensCorrections", "Enable Lens Corrections");
        public static readonly IDevelopControllerParameter<bool> Effects                          = new BoolParameter("EnableEffects", "Enable Effects");
        public static readonly IDevelopControllerParameter<bool> Calibration                      = new BoolParameter("EnableCalibration", "Enable Camera Calibration");
        public static readonly IDevelopControllerParameter<bool> Retouch                          = new BoolParameter("EnableRetouch", "Enable Spot Removal");
        public static readonly IDevelopControllerParameter<bool> RedEye                           = new BoolParameter("EnableRedEye", "Enable Red Eye");
        public static readonly IDevelopControllerParameter<bool> GradientBasedCorrections         = new BoolParameter("EnableGradientBasedCorrections", "Enable Graduated Filter");
        public static readonly IDevelopControllerParameter<bool> CircularGradientBasedCorrections = new BoolParameter("EnableCircularGradientBasedCorrections", "Enable Radial Filter");
        public static readonly IDevelopControllerParameter<bool> PaintBasedCorrections            = new BoolParameter("EnablePaintBasedCorrections", "Enable Adjustment Brush");
        public static readonly IDevelopControllerParameter<bool> GrayscaleMix                     = new BoolParameter("EnableGrayscaleMix", "Enable Black & White Mix");

        static EnablePanelParameter()
        {
            AddParameters(ToneCurve, ColorAdjustments, SplitToning, Detail, LensCorrections, Effects, Calibration,
                Retouch, RedEye, GradientBasedCorrections, CircularGradientBasedCorrections, PaintBasedCorrections,
                GrayscaleMix);
        }

        private EnablePanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class BoolParameter : EnablePanelParameter, IDevelopControllerParameter<bool>
        {
            public BoolParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}