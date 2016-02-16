namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class SplitToningPanelParameter : ParameterGroup
    {
        public readonly IParameter<int> SplitToningHighlightHue        = new Parameter<int>("SplitToningHighlightHue", "Highlights: Hue");
        public readonly IParameter<int> SplitToningHighlightSaturation = new Parameter<int>("SplitToningHighlightSaturation", "Highlights: Saturation");
        public readonly IParameter<int> SplitToningBalance             = new Parameter<int>("SplitToningBalance", "Balance");
        public readonly IParameter<int> SplitToningShadowHue           = new Parameter<int>("SplitToningShadowHue", "Shadows: Hue");
        public readonly IParameter<int> SplitToningShadowSaturation    = new Parameter<int>("SplitToningShadowSaturation", "Shadows: Saturation");

        internal SplitToningPanelParameter()
        {
            AddParameters(SplitToningHighlightHue, SplitToningHighlightSaturation, SplitToningBalance,
                SplitToningShadowHue, SplitToningShadowSaturation);
        }
    }
}