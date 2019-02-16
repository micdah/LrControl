using System;
using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class UnaryOperatorParameterFunctionFactory : FunctionFactory
    {
        public IParameter Parameter { get; }
        public UnaryOperation Operation { get; }

        public UnaryOperatorParameterFunctionFactory(ISettings settings, ILrApi api, IParameter parameter,
            UnaryOperation operation) : base(settings, api)
        {
            Parameter = parameter;
            Operation = operation;
            switch (operation)
            {
                case UnaryOperation.Increment:
                    DisplayName = $"Increment {parameter.DisplayName}";
                    Key = $"Increment{parameter.Name}";
                    break;
                case UnaryOperation.Decrement:
                    DisplayName = $"Decrement {parameter.DisplayName}";
                    Key = $"Decrement{parameter.Name}";
                    break;
                default:
                    throw new ArgumentException("Unsupported unary operation", nameof(operation));
            }
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new UnaryOperatorParameterFunction(settings, api, DisplayName, Key, Parameter, Operation);
    }
}