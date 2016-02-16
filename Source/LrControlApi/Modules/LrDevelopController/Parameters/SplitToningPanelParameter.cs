using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class SplitToningPanelParameter : ParameterGroup<SplitToningPanelParameter>
    {
        public static readonly IParameter<int> SplitToningHighlightHue        = new Parameter<int>("SplitToningHighlightHue", "Highlights: Hue");
        public static readonly IParameter<int> SplitToningHighlightSaturation = new Parameter<int>("SplitToningHighlightSaturation", "Highlights: Saturation");
        public static readonly IParameter<int> SplitToningBalance             = new Parameter<int>("SplitToningBalance", "Balance");
        public static readonly IParameter<int> SplitToningShadowHue           = new Parameter<int>("SplitToningShadowHue", "Shadows: Hue");
        public static readonly IParameter<int> SplitToningShadowSaturation    = new Parameter<int>("SplitToningShadowSaturation", "Shadows: Saturation");

        static SplitToningPanelParameter()
        {
            AddParameters(SplitToningHighlightHue, SplitToningHighlightSaturation, SplitToningBalance,
                SplitToningShadowHue, SplitToningShadowSaturation);
        }
    }
}