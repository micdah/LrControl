using System;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class AdjustPanelParameter : ParameterGroup<AdjustPanelParameter>
    {
        public static readonly IParameter<WhiteBalanceValue> WhiteBalance    = new Parameter<WhiteBalanceValue>("WhiteBalance", "Whitebalance");
        public static readonly IParameter<int> Temperature                   = new Parameter<int>("Temperature", "Whitebalance: Temperature");
        public static readonly IParameter<int> Tint                          = new Parameter<int>("Tint", "Whitebalance: Tint");
        public static readonly IParameter<double> Exposure                   = new Parameter<double>("Exposure", "Tone: Exposure");
        public static readonly IParameter<int> Contrast                      = new Parameter<int>("Contrast", "Tone: Contrast");
        public static readonly IParameter<int> Highlights                    = new Parameter<int>("Highlights", "Tone: Highlights");
        public static readonly IParameter<int> Shadows                       = new Parameter<int>("Shadows", "Tone: Shadows");
        public static readonly IParameter<int> Whites                        = new Parameter<int>("Whites", "Tone: Whites");
        public static readonly IParameter<int> Blacks                        = new Parameter<int>("Blacks", "Tone: Blacks");
        public static readonly IParameter<int> Clarity                       = new Parameter<int>("Clarity", "Presence: Clarity");
        public static readonly IParameter<int> Vibrance                      = new Parameter<int>("Vibrance", "Presence: Vibrance");
        public static readonly IParameter<int> Saturation                    = new Parameter<int>("Saturation", "Presence: Saturation");

        static AdjustPanelParameter()
        {
            AddParameters(WhiteBalance, Temperature, Tint, Exposure, Contrast, Highlights, Shadows, Whites, Blacks,
                Clarity, Vibrance, Saturation);
        }
    }
}