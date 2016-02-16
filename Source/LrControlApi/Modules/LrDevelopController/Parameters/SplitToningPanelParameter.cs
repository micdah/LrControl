using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class SplitToningPanelParameter : Parameter<SplitToningPanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> SplitToningHighlightHue        = new IntParameter("SplitToningHighlightHue", "Highlights: Hue");
        public static readonly IDevelopControllerParameter<int> SplitToningHighlightSaturation = new IntParameter("SplitToningHighlightSaturation", "Highlights: Saturation");
        public static readonly IDevelopControllerParameter<int> SplitToningBalance             = new IntParameter("SplitToningBalance", "Balance");
        public static readonly IDevelopControllerParameter<int> SplitToningShadowHue           = new IntParameter("SplitToningShadowHue", "Shadows: Hue");
        public static readonly IDevelopControllerParameter<int> SplitToningShadowSaturation    = new IntParameter("SplitToningShadowSaturation", "Shadows: Saturation");

        static SplitToningPanelParameter()
        {
            AddParameters(SplitToningHighlightHue, SplitToningHighlightSaturation, SplitToningBalance,
                SplitToningShadowHue, SplitToningShadowSaturation);
        }

        private SplitToningPanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : SplitToningPanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}