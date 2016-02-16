using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class TonePanelParameter : ParameterGroup<TonePanelParameter>
    {
        public static readonly IParameter<int> ParametricHighlights     = new Parameter<int>("ParametricHighlights", "Highlights");
        public static readonly IParameter<int> ParametricLights         = new Parameter<int>("ParametricLights", "Lights");
        public static readonly IParameter<int> ParametricDarks          = new Parameter<int>("ParametricDarks", "Darks");
        public static readonly IParameter<int> ParametricShadows        = new Parameter<int>("ParametricShadows", "Shadows");
        public static readonly IParameter<int> ParametricShadowSplit    = new Parameter<int>("ParametricShadowSplit", "Shadow Split");
        public static readonly IParameter<int> ParametricMidtoneSplit   = new Parameter<int>("ParametricMidtoneSplit", "Midtone Split");
        public static readonly IParameter<int> ParametricHighlightSplit = new Parameter<int>("ParametricHighlightSplit", "Highlight Split");

        static TonePanelParameter()
        {
            AddParameters(ParametricHighlights, ParametricLights, ParametricDarks, ParametricShadows,
                ParametricShadowSplit, ParametricMidtoneSplit, ParametricHighlightSplit);
        }
    }
}