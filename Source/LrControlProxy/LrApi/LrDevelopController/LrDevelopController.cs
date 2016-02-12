using System;
using LrControlProxy.LrApi.LrDevelopController.Parameters;

namespace LrControlProxy.LrApi.LrDevelopController
{
    public class LrDevelopController
    {
        public event AdjustmentChange AdjustmentChangeObserver;


        protected virtual void OnAdjustmentChangeObserver(string parameter, int newvalue)
        {
            AdjustmentChangeObserver?.Invoke(parameter, newvalue);
        }
    }
}