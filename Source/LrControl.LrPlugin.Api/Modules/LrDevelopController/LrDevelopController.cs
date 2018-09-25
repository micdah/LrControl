using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Communication;

// ReSharper disable DelegateSubtraction

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    internal class LrDevelopController : ModuleBase<LrDevelopController>, ILrDevelopController
    {
        private readonly ConcurrentDictionary<IParameter, ParameterChangedHandler> _parameterChangedHandlers;
        private readonly Dictionary<string, IParameter> _parameterLookup;

        public LrDevelopController(MessageProtocol<LrDevelopController> messageProtocol) : base(messageProtocol)
        {
            _parameterLookup = new Dictionary<string, IParameter>();
            foreach (var parameter in Parameters.Parameters.AllParameters)
            {
                _parameterLookup[parameter.Name] = parameter;
            }

            _parameterChangedHandlers = new ConcurrentDictionary<IParameter, ParameterChangedHandler>();
        }

        public void AddParameterChangedListener(IParameter parameter, ParameterChangedHandler handler)
        {
            _parameterChangedHandlers.AddOrUpdate(parameter,
                p => handler,
                (p, parameterChanged) => parameterChanged + handler);
        }

        public void RemoveParameterChangedListener(IParameter parameter, ParameterChangedHandler handler)
        {
            _parameterChangedHandlers.AddOrUpdate(parameter,
                p => null,
                (p, parameterChanged) => parameterChanged - handler);
        }

        public bool Decrement(IParameter param)
        {
            return Invoke(nameof(Decrement), param);
        }

        public bool GetProcessVersion(out ProcessVersion processVersion)
        {
            if (!Invoke(out string result, nameof(GetProcessVersion)))
                return False(out processVersion);

            processVersion = ProcessVersion.GetEnumForValue(result);
            return processVersion != null;
        }

        public bool GetRange(out Range range, IParameter param)
        {
            if (!Invoke(out double min, out double max, nameof(GetRange), param))
                return False(out range);

            range = new Range(min, max);
            return true;
        }

        public bool GetSelectedTool(out Tool tool)
        {
            if (!Invoke(out string result, nameof(GetSelectedTool)))
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

        public bool GetValue<TEnum,TValue>(out TEnum value, IEnumerationParameter<TValue> param)
            where TEnum : IEnumeration<TValue>
            where TValue : IComparable 
        {
            if (Invoke(out TValue result, nameof(GetValue), param))
            {
                value = Enumeration<TEnum,TValue>.GetEnumForValue(result);
                return true;
            }
            value = default;
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

        public bool RevealPanel(Panel panel)
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

        public bool SetValue<TValue>(IEnumerationParameter<TValue> param, IEnumeration<TValue> value) 
            where TValue : IComparable
        {
            return Invoke(nameof(SetValue), param, value);
        }

        public bool StartTracking(IParameter param)
        {
            return Invoke(nameof(StartTracking), param);
        }

        public bool StopTracking()
        {
            return Invoke(nameof(StopTracking));
        }

        public void OnParametersChanged(string parameterNames)
        {
            foreach (var parameterName in parameterNames.Split(','))
            {
                if (!_parameterLookup.TryGetValue(parameterName, out var parameter)) return;

                if (_parameterChangedHandlers.TryGetValue(parameter, out var handler))
                {
                    handler(parameter);
                }
            }
        }
    }
}