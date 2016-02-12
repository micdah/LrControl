using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    internal class LrDevelopController : ILrDevelopController
    {
        public event AdjustmentChange AdjustmentChangeObserver;
        public void Decrement(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public ProcessVersion GetProcessVersion()
        {
            throw new System.NotImplementedException();
        }

        public Range GetRange(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public Tool GetSelectedTool()
        {
            throw new System.NotImplementedException();
        }

        public object GetValue(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public void Increment(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public void ResetAllDevelopAdjustments()
        {
            throw new System.NotImplementedException();
        }

        public void ResetBrushing()
        {
            throw new System.NotImplementedException();
        }

        public void ResetCircularGradient()
        {
            throw new System.NotImplementedException();
        }

        public void ResetCrop()
        {
            throw new System.NotImplementedException();
        }

        public void ResetGradient()
        {
            throw new System.NotImplementedException();
        }

        public void ResetRedEye()
        {
            throw new System.NotImplementedException();
        }

        public void ResetSpotRemoval()
        {
            throw new System.NotImplementedException();
        }

        public void ResetToDefault(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public void RevealAdjustedControls(bool reveal)
        {
            throw new System.NotImplementedException();
        }

        public void RevealPanel(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public void Revealpanel(Panel panel)
        {
            throw new System.NotImplementedException();
        }

        public void SelectTool(Tool tool)
        {
            throw new System.NotImplementedException();
        }

        public void SetMultipleAdjustmentThreshold(double seconds)
        {
            throw new System.NotImplementedException();
        }

        public void SetProcessVersion(ProcessVersion version)
        {
            throw new System.NotImplementedException();
        }

        public void SetTrackingDelay(double seconds)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue(IDevelopControllerParameter param, object value)
        {
            throw new System.NotImplementedException();
        }

        public void StartTracking(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public void StopTracking()
        {
            throw new System.NotImplementedException();
        }
    }
}