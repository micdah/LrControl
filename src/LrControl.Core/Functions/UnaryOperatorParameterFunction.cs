using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class UnaryOperatorParameterFunction : Function
    {
        private readonly IParameter _parameter;
        private readonly UnaryOperation _operation;

        public UnaryOperatorParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter parameter, UnaryOperation operation) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
            _operation = operation;
        }

        public override void Apply(int value, Range range)
        {
            if (!range.IsMaximum(value)) return;

            switch (_operation)
            {
                case UnaryOperation.Increment:
                    Api.LrDevelopController.Increment(_parameter);
                    ShowHud($"Incremented {_parameter.DisplayName}");
                    break;
                case UnaryOperation.Decrement:
                    Api.LrDevelopController.Decrement(_parameter);
                    ShowHud($"Decremented {_parameter.DisplayName}");
                    break;
            }
        }
    }
}