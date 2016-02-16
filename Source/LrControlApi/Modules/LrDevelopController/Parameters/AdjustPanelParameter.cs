using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class AdjustPanelParameter : Parameter<AdjustPanelParameter>
    {
        public static readonly IDevelopControllerParameter<WhiteBalanceValue> WhiteBalance = new WhiteBalanceParameter("WhiteBalance", "Whitebalance");
        public static readonly IDevelopControllerParameter<int> Temperature                = new IntParameter("Temperature", "Whitebalance: Temperature");
        public static readonly IDevelopControllerParameter<int> Tint                       = new IntParameter("Tint", "Whitebalance: Tint");
        public static readonly IDevelopControllerParameter<int> Exposure                   = new IntParameter("Exposure", "Tone: Exposure");
        public static readonly IDevelopControllerParameter<int> Contrast                   = new IntParameter("Contrast", "Tone: Contrast");
        public static readonly IDevelopControllerParameter<int> Highlights                 = new IntParameter("Highlights", "Tone: Highlights");
        public static readonly IDevelopControllerParameter<int> Shadows                    = new IntParameter("Shadows", "Tone: Shadows");
        public static readonly IDevelopControllerParameter<int> Whites                     = new IntParameter("Whites", "Tone: Whites");
        public static readonly IDevelopControllerParameter<int> Blacks                     = new IntParameter("Blacks", "Tone: Blacks");
        public static readonly IDevelopControllerParameter<int> Clarity                    = new IntParameter("Clarity", "Presence: Clarity");
        public static readonly IDevelopControllerParameter<int> Vibrance                   = new IntParameter("Vibrance", "Presence: Vibrance");
        public static readonly IDevelopControllerParameter<int> Saturation                 = new IntParameter("Saturation", "Presence: Saturation");

        static AdjustPanelParameter()
        {
            AddParameters(WhiteBalance, Temperature, Tint, Exposure, Contrast, Highlights, Shadows, Whites, Blacks,
                Clarity, Vibrance, Saturation);
        }
        
        private AdjustPanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        public class WhiteBalanceValue : ClassEnum<string,WhiteBalanceValue>
        {
            public static readonly WhiteBalanceValue AsShot      = new WhiteBalanceValue("As Shot");
            public static readonly WhiteBalanceValue Auto        = new WhiteBalanceValue("Auto");
            public static readonly WhiteBalanceValue Cloudy      = new WhiteBalanceValue("Cloudy");
            public static readonly WhiteBalanceValue Daylight    = new WhiteBalanceValue("Daylight");
            public static readonly WhiteBalanceValue Flash       = new WhiteBalanceValue("Flash");
            public static readonly WhiteBalanceValue Flourescent = new WhiteBalanceValue("Flourescent");
            public static readonly WhiteBalanceValue Shade       = new WhiteBalanceValue("Shade");
            public static readonly WhiteBalanceValue Tungsten    = new WhiteBalanceValue("Tungsten");

            static WhiteBalanceValue()
            {
                AddEnums(AsShot, Auto, Cloudy, Daylight, Flash, Flourescent, Shade, Tungsten);
            }

            private WhiteBalanceValue(string name) : base(name, name)
            {
            }
        }

        private class WhiteBalanceParameter : AdjustPanelParameter, IDevelopControllerParameter<WhiteBalanceValue>
        {
            public WhiteBalanceParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }

        private class IntParameter : AdjustPanelParameter, IDevelopControllerParameter<Int32>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}