using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public class Panel : ClassEnum<string,Panel>
    {
        public static readonly Panel Basic                     = new Panel("Basic", "adjustPanel");
        public static readonly Panel ToneCurve                 = new Panel("Tone Curve", "tonePanel");
        public static readonly Panel ColorAdjustment           = new Panel("Color Adjustment", "mixerPanel");
        public static readonly Panel SplitToning               = new Panel("Split Toning", "splitToningPanel");
        public static readonly Panel Detail                    = new Panel("Detail", "detailPanel");
        public static readonly Panel LensCorrections           = new Panel("Lens Corrections", "lensCorrectionsPanel");
        public static readonly Panel Effects                   = new Panel("Effects", "effectsPanel");
        public static readonly Panel CameraCalibration         = new Panel("Camera Calibration", "calibratePanel");

        private Panel(string name, string value) : base(name, value)
        {
        }
    }
}