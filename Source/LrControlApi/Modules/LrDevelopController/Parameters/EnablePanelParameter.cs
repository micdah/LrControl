namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EnablePanelParameter : Parameter<EnablePanelParameter>, IDevelopControllerParameter
    {
        public static readonly EnablePanelParameter ToneCurve                        = new EnablePanelParameter("EnableToneCurve", "Enable Tone Curve");
        public static readonly EnablePanelParameter ColorAdjustments                 = new EnablePanelParameter("EnableColorAdjustments", "Enable Color Adjustments");
        public static readonly EnablePanelParameter SplitToning                      = new EnablePanelParameter("EnableSplitToning", "Enable Split Toning");
        public static readonly EnablePanelParameter Detail                           = new EnablePanelParameter("EnableDetail", "Enable Detail");
        public static readonly EnablePanelParameter LensCorrections                  = new EnablePanelParameter("EnableLensCorrections", "Enable Lens Corrections");
        public static readonly EnablePanelParameter Effects                          = new EnablePanelParameter("EnableEffects", "Enable Effects");
        public static readonly EnablePanelParameter Calibration                      = new EnablePanelParameter("EnableCalibration", "Enable Camera Calibration");
        public static readonly EnablePanelParameter Retouch                          = new EnablePanelParameter("EnableRetouch", "Enable Spot Removal");
        public static readonly EnablePanelParameter RedEye                           = new EnablePanelParameter("EnableRedEye", "Enable Red Eye");
        public static readonly EnablePanelParameter GradientBasedCorrections         = new EnablePanelParameter("EnableGradientBasedCorrections", "Enable Graduated Filter");
        public static readonly EnablePanelParameter CircularGradientBasedCorrections = new EnablePanelParameter("EnableCircularGradientBasedCorrections", "Enable Radial Filter");
        public static readonly EnablePanelParameter PaintBasedCorrections            = new EnablePanelParameter("EnablePaintBasedCorrections", "Enable Adjustment Brush");
        public static readonly EnablePanelParameter GrayscaleMix                     = new EnablePanelParameter("EnableGrayscaleMix", "Enable Black & White Mix");

        static EnablePanelParameter()
        {
            AddParameters(ToneCurve, ColorAdjustments, SplitToning, Detail, LensCorrections, Effects, Calibration,
                Retouch, RedEye, GradientBasedCorrections, CircularGradientBasedCorrections, PaintBasedCorrections,
                GrayscaleMix);
        }

        private EnablePanelParameter(string name, string displayName) : base(name, displayName, typeof(bool))
        {
        }
    }
}