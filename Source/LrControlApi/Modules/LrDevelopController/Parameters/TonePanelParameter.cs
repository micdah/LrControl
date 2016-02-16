namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class TonePanelParameter : ParameterGroup
    {
        public readonly IParameter<int> ParametricHighlights     = new Parameter<int>("ParametricHighlights", "Highlights");
        public readonly IParameter<int> ParametricLights         = new Parameter<int>("ParametricLights", "Lights");
        public readonly IParameter<int> ParametricDarks          = new Parameter<int>("ParametricDarks", "Darks");
        public readonly IParameter<int> ParametricShadows        = new Parameter<int>("ParametricShadows", "Shadows");
        public readonly IParameter<int> ParametricShadowSplit    = new Parameter<int>("ParametricShadowSplit", "Shadow Split");
        public readonly IParameter<int> ParametricMidtoneSplit   = new Parameter<int>("ParametricMidtoneSplit", "Midtone Split");
        public readonly IParameter<int> ParametricHighlightSplit = new Parameter<int>("ParametricHighlightSplit", "Highlight Split");

        internal TonePanelParameter()
        {
            AddParameters(ParametricHighlights, ParametricLights, ParametricDarks, ParametricShadows,
                ParametricShadowSplit, ParametricMidtoneSplit, ParametricHighlightSplit);
        }
    }
}