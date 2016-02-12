namespace LrControlApi.LrDevelopController
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