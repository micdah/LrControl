using System.Collections.Generic;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public static class Parameters
    {
        public static readonly AdjustPanelParameter AdjustPanelParameters                   = new AdjustPanelParameter();
        public static readonly CalibratePanelParameter CalibratePanelParameters             = new CalibratePanelParameter();
        public static readonly CropParameter CropParameters                                 = new CropParameter();
        public static readonly DetailPanelParameter DetailPanelParameters                   = new DetailPanelParameter();
        public static readonly EffectsPanelParameter EffectsPanelParameters                 = new EffectsPanelParameter();
        public static readonly EnablePanelParameter EnablePanelParameters                   = new EnablePanelParameter();
        public static readonly LensCorrectionsPanelParameter LensCorrectionsPanelParameters = new LensCorrectionsPanelParameter();
        public static readonly LocalizedAdjustmentsParameter LocalizedAdjustmentsParameters = new LocalizedAdjustmentsParameter();
        public static readonly MixerPanelParameter MixerPanelParameters                     = new MixerPanelParameter();
        public static readonly SplitToningPanelParameter SplitToningPanelParameters          = new SplitToningPanelParameter();
        public static readonly TonePanelParameter TonePanelParameters                       = new TonePanelParameter();

        public static IList<ParameterGroup> AllGroups =>

            new List<ParameterGroup>
            {
                AdjustPanelParameters,
                CalibratePanelParameters,
                CropParameters,
                DetailPanelParameters,
                EffectsPanelParameters,
                EnablePanelParameters,
                LensCorrectionsPanelParameters,
                LocalizedAdjustmentsParameters,
                MixerPanelParameters,
                SplitToningPanelParameters,
                TonePanelParameters
            };
    }
}