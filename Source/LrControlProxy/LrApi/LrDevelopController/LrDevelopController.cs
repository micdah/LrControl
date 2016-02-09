using System;
using LrControlProxy.LrApi.LrDevelopController.Parameters;

namespace LrControlProxy.LrApi.LrDevelopController
{
    public delegate void AdjustmentChange(String parameter, Int32 newValue);

    public interface ILrDevelopController
    {
        event AdjustmentChange AdjustmentChangeObserver;

        void Decrement(AdjustPanelParameter parameter);
        void Decrement(CalibratePanelParameter parameter);
        void Decrement(CropAngleParameter paramter);
        void Decrement(DetailPanelParameter parameter);
        void Decrement(EffectsPanelParameter parameter);
        void Decrement(LensCorrectionsPanelParameter parameter);
        void Decrement(LocalizedAdjustmentsParameter parameter);
        void Decrement(MixerPanelParameter parameter);
        void Decrement(SplitToningPanelParameter parameter);
        void Decrement(TonePanelParameter parameter);
    }

    public class LrDevelopController
    {
        public event AdjustmentChange AdjustmentChangeObserver;


        protected virtual void OnAdjustmentChangeObserver(string parameter, int newvalue)
        {
            AdjustmentChangeObserver?.Invoke(parameter, newvalue);
        }
    }
}