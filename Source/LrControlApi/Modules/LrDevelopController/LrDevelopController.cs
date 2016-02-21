using System.Collections.Generic;
using System.Linq;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Communication;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    internal class LrDevelopController : ModuleBase<LrDevelopController>, ILrDevelopController
    {
        private readonly Dictionary<string, IParameter> _parameterLookup;

        public LrDevelopController(MessageProtocol<LrDevelopController> messageProtocol) : base(messageProtocol)
        {
            _parameterLookup = new Dictionary<string, IParameter>();
            foreach (var parameter in Parameters.Parameters.AllParameters)
            {
                _parameterLookup[parameter.Name] = parameter;
            }
        }

        public event ParameterChangedHandler ParameterChanged;

        public void OnParameterChanged(string parameterName)
        {
            IParameter parameter;
            if (_parameterLookup.TryGetValue(parameterName, out parameter))
            {
                ParameterChanged?.Invoke(parameter);
            }
        }

        public bool Decrement(IParameter param)
        {
            return Invoke(nameof(Decrement), param);
        }

        public bool GetProcessVersion(out ProcessVersion processVersion)
        {
            string result;
            if (!Invoke(out result, nameof(GetProcessVersion)))
                return False(out processVersion);

            processVersion = ProcessVersion.GetEnumForValue(result);
            return processVersion != null;
        }

        public bool GetRange(out Range range, IParameter param)
        {
            double min, max;
            if (!Invoke(out min, out max, nameof(GetRange), param))
                return False(out range);

            range = new Range(min, max);
            return true;
        }

        public bool GetSelectedTool(out Tool tool)
        {
            string result;
            if (!Invoke(out result, nameof(GetSelectedTool)))
                return False(out tool);

            tool = Tool.GetEnumForValue(result);
            return tool != null;
        }

        public bool GetValue(out int value, IParameter<int> param)
        {
            return Invoke(out value, nameof(GetValue), param);
        }

        public bool GetValue(out double value, IParameter<double> param)
        {
            return Invoke(out value, nameof(GetValue), param);
        }

        public bool GetValue(out bool value, IParameter<bool> param)
        {
            return Invoke(out value, nameof(GetValue), param);
        }

        public bool GetValue(out string value, IParameter<string> param)
        {
            return Invoke(out value, nameof(GetValue), param);
        }

        public bool GetValue<TEnum,TValue>(out TEnum value, IParameter<ClassEnum<TValue, TEnum>> param)
            where TEnum : ClassEnum<TValue,TEnum>
        {
            TValue result;
            if (Invoke(out result, nameof(GetValue), param))
            {
                value = ClassEnum<TValue, TEnum>.GetEnumForValue(result);
                return true;
            }
            value = null;
            return false;
        }

        public bool Increment(IParameter param)
        {
            return Invoke(nameof(Increment), param);
        }

        public bool ResetAllDevelopAdjustments()
        {
            return Invoke(nameof(ResetAllDevelopAdjustments));
        }

        public bool ResetBrushing()
        {
            return Invoke(nameof(ResetBrushing));
        }

        public bool ResetCircularGradient()
        {
            return Invoke(nameof(ResetCircularGradient));
        }

        public bool ResetCrop()
        {
            return Invoke(nameof(ResetCrop));
        }

        public bool ResetGradient()
        {
            return Invoke(nameof(ResetGradient));
        }

        public bool ResetRedEye()
        {
            return Invoke(nameof(ResetRedEye));
        }

        public bool ResetSpotRemoval()
        {
            return Invoke(nameof(ResetSpotRemoval));
        }

        public bool ResetToDefault(IParameter param)
        {
            return Invoke(nameof(ResetToDefault), param);
        }

        public bool RevealAdjustedControls(bool reveal)
        {
            return Invoke(nameof(RevealAdjustedControls), reveal);
        }

        public bool RevealPanel(IParameter param)
        {
            return Invoke(nameof(RevealPanel), param);
        }

        public bool Revealpanel(Panel panel)
        {
            return Invoke(nameof(RevealPanel), panel);
        }

        public bool SelectTool(Tool tool)
        {
            return Invoke(nameof(SelectTool), tool);
        }

        public bool SetMultipleAdjustmentThreshold(double seconds)
        {
            return Invoke(nameof(SetMultipleAdjustmentThreshold), seconds);
        }

        public bool SetProcessVersion(ProcessVersion version)
        {
            return Invoke(nameof(SetProcessVersion), version.Value);
        }

        public bool SetTrackingDelay(double seconds)
        {
            return Invoke(nameof(SetTrackingDelay), seconds);
        }

        public bool SetValue(IParameter<int> param, int value)
        {
            return Invoke(nameof(SetValue), param, value);
        }

        public bool SetValue(IParameter<double> param, double value)
        {
            return Invoke(nameof(SetValue), param, value);
        }

        public bool SetValue(IParameter<bool> param, bool value)
        {
            return Invoke(nameof(SetValue), param, value);
        }

        public bool SetValue(IParameter<string> param, string value)
        {
            return Invoke(nameof(SetValue), param, value);
        }

        public bool SetValue<TEnum,TValue>(IParameter<TEnum> param, ClassEnum<TValue,TEnum> enumClass)
            where TEnum : ClassEnum<TValue,TEnum>
        {
            return Invoke(nameof(SetValue), param, enumClass);
        }

        public bool StartTracking(IParameter param)
        {
            return Invoke(nameof(StartTracking), param);
        }

        public bool StopTracking()
        {
            return Invoke(nameof(StopTracking));
        }
    }
}