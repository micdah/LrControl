using LrControlApi.Common;
using LrControlApi.Communication;

namespace LrControlApi.LrDevelopController
{
    internal class LrDevelopController : ModuleBase<LrDevelopController>, ILrDevelopController
    {
        public LrDevelopController(MessageProtocol<LrDevelopController> messageProtocol) : base(messageProtocol)
        {
        }

        public event AdjustmentChange AdjustmentChangeObserver;
        public void Decrement(IDevelopControllerParameter param)
        {
            Invoke(nameof(Decrement), param);
        }

        public ProcessVersion GetProcessVersion()
        {
            return InvokeWithResult<ProcessVersion,string>(nameof(GetProcessVersion));;
        }

        public Range GetRange(IDevelopControllerParameter param)
        {
            throw new System.NotImplementedException();
        }

        public Tool GetSelectedTool()
        {
            return InvokeWithResult<Tool, string>(nameof(GetSelectedTool));
        }

        public object GetValue(IDevelopControllerParameter param)
        {
            return InvokeWithResult(nameof(GetValue), param);
        }

        public void Increment(IDevelopControllerParameter param)
        {
            Invoke(nameof(Increment), param);
        }

        public void ResetAllDevelopAdjustments()
        {
            Invoke(nameof(ResetAllDevelopAdjustments));
        }

        public void ResetBrushing()
        {
            Invoke(nameof(ResetBrushing));
        }

        public void ResetCircularGradient()
        {
            Invoke(nameof(ResetCircularGradient));
        }

        public void ResetCrop()
        {
            Invoke(nameof(ResetCrop));
        }

        public void ResetGradient()
        {
            Invoke(nameof(ResetGradient));
        }

        public void ResetRedEye()
        {
            Invoke(nameof(ResetRedEye));
        }

        public void ResetSpotRemoval()
        {
            Invoke(nameof(ResetSpotRemoval));
        }

        public void ResetToDefault(IDevelopControllerParameter param)
        {
            Invoke(nameof(ResetToDefault), param);
        }

        public void RevealAdjustedControls(bool reveal)
        {
            Invoke(nameof(RevealAdjustedControls), reveal);
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