using LrControl.Configurations;
using LrControl.Enums;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class UnaryOperatorParameterFunction : Function
    {
        public IParameter Parameter { get; }
        public UnaryOperation Operation { get; }

        public UnaryOperatorParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter parameter, UnaryOperation operation) : base(settings, api, displayName, key)
        {
            Parameter = parameter;
            Operation = operation;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;

            switch (Operation)
            {
                case UnaryOperation.Increment:
                    Api.LrDevelopController.Increment(Parameter);
                    ShowHud($"Incremented {Parameter.DisplayName}");
                    break;
                case UnaryOperation.Decrement:
                    Api.LrDevelopController.Decrement(Parameter);
                    ShowHud($"Decremented {Parameter.DisplayName}");
                    break;
            }
        }
    }
}