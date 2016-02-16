using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public class Panel : ClassEnum<string,Panel>
    {
        public static readonly Panel Basic                     = new Panel("adjustPanel", "Basic");
        public static readonly Panel ToneCurve                 = new Panel("tonePanel", "Tone Curve");
        public static readonly Panel ColorAdjustment           = new Panel("mixerPanel", "Color Adjustment");
        public static readonly Panel SplitToning               = new Panel("splitToningPanel", "Split Toning");
        public static readonly Panel Detail                    = new Panel("detailPanel", "Detail");
        public static readonly Panel LensCorrections           = new Panel("lensCorrectionsPanel", "Lens Corrections");
        public static readonly Panel Effects                   = new Panel("effectsPanel", "Effects");
        public static readonly Panel CameraCalibration         = new Panel("calibratePanel", "Camera Calibration");

        static Panel()
        {
            AddEnums(Basic, ToneCurve, ColorAdjustment, SplitToning, Detail, LensCorrections, Effects, CameraCalibration);
        }

        private Panel(string value, string name) : base(value, name)
        {
        }
    }
}