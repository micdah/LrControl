namespace LrControlProxy.LrApi.LrDevelopController.Parameters
{
    public class TonePanelParameter : Parameter<TonePanelParameter>, IDevelopControllerParameter
    {
        public static readonly TonePanelParameter ParametricHighlights     = new TonePanelParameter("ParametricHighlights", "Highlights");
        public static readonly TonePanelParameter ParametricLights         = new TonePanelParameter("ParametricLights", "Lights");
        public static readonly TonePanelParameter ParametricDarks          = new TonePanelParameter("ParametricDarks", "Darks");
        public static readonly TonePanelParameter ParametricShadows        = new TonePanelParameter("ParametricShadows", "Shadows");
        public static readonly TonePanelParameter ParametricShadowSplit    = new TonePanelParameter("ParametricShadowSplit", "Shadow Split");
        public static readonly TonePanelParameter ParametricMidtoneSplit   = new TonePanelParameter("ParametricMidtoneSplit", "Midtone Split");
        public static readonly TonePanelParameter ParametricHighlightSplit = new TonePanelParameter("ParametricHighlightSplit", "Highlight Split");

        private TonePanelParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }
    }
}