using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class TonePanelParameter : Parameter<TonePanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> ParametricHighlights     = new IntParameter("ParametricHighlights", "Highlights");
        public static readonly IDevelopControllerParameter<int> ParametricLights         = new IntParameter("ParametricLights", "Lights");
        public static readonly IDevelopControllerParameter<int> ParametricDarks          = new IntParameter("ParametricDarks", "Darks");
        public static readonly IDevelopControllerParameter<int> ParametricShadows        = new IntParameter("ParametricShadows", "Shadows");
        public static readonly IDevelopControllerParameter<int> ParametricShadowSplit    = new IntParameter("ParametricShadowSplit", "Shadow Split");
        public static readonly IDevelopControllerParameter<int> ParametricMidtoneSplit   = new IntParameter("ParametricMidtoneSplit", "Midtone Split");
        public static readonly IDevelopControllerParameter<int> ParametricHighlightSplit = new IntParameter("ParametricHighlightSplit", "Highlight Split");

        static TonePanelParameter()
        {
            AddParameters(ParametricHighlights, ParametricLights, ParametricDarks, ParametricShadows,
                ParametricShadowSplit, ParametricMidtoneSplit, ParametricHighlightSplit);
        }

        private TonePanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : TonePanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}