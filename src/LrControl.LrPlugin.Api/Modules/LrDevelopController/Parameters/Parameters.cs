using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public static class Parameters
    {
        private static readonly Lazy<ReadOnlyCollection<ParameterGroup>> AllGroupsCache = new Lazy<ReadOnlyCollection<ParameterGroup>>(GetGroups);
        private static readonly Lazy<ReadOnlyCollection<IParameter>> AllParametersCache = new Lazy<ReadOnlyCollection<IParameter>>(GetParameters);

        public static readonly AdjustPanelParameter AdjustPanelParameters                    = new AdjustPanelParameter();
        public static readonly CalibratePanelParameter CalibratePanelParameters              = new CalibratePanelParameter();
        public static readonly CropParameter CropParameters                                  = new CropParameter();
        public static readonly DetailPanelParameter DetailPanelParameters                    = new DetailPanelParameter();
        public static readonly EffectsPanelParameter EffectsPanelParameters                  = new EffectsPanelParameter();
        public static readonly EnablePanelParameter EnablePanelParameters                    = new EnablePanelParameter();
        public static readonly LensCorrectionsPanelParameter LensCorrectionsPanelParameters  = new LensCorrectionsPanelParameter();
        public static readonly LocalizedAdjustmentsParameter LocalizedAdjustmentsParameters  = new LocalizedAdjustmentsParameter();
        public static readonly MixerPanelParameter MixerPanelParameters                      = new MixerPanelParameter();
        public static readonly SplitToningPanelParameter SplitToningPanelParameters          = new SplitToningPanelParameter();
        public static readonly TonePanelParameter TonePanelParameters                        = new TonePanelParameter();

        public static IList<ParameterGroup> AllGroups => AllGroupsCache.Value;

        public static IList<IParameter> AllParameters => AllParametersCache.Value;

        private static ReadOnlyCollection<ParameterGroup> GetGroups()
        {
            return new ReadOnlyCollection<ParameterGroup>(new List<ParameterGroup>
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
            });
        }

        private static ReadOnlyCollection<IParameter> GetParameters()
        {
            var parameters = new List<IParameter>();
            foreach (var group in AllGroups)
            {
                parameters.AddRange(group.AllParameters);
            }
            return new ReadOnlyCollection<IParameter>(parameters);
        }
    }
}