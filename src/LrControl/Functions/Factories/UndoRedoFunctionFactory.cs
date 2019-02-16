using LrControl.Configurations;
using LrControl.Enums;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class UndoRedoFunctionFactory : FunctionFactory
    {
        public Operation Operation { get; }

        public UndoRedoFunctionFactory(ISettings settings, ILrApi api, Operation operation) 
            : base(settings, api)
        {
            Operation = operation;
            DisplayName = operation.ToString();
            Key = operation.ToString();
        }

        public override string DisplayName { get; }
        public override string Key { get; }
        
        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new UndoRedoFunction(settings, api, DisplayName, Key, Operation);
        }
    }
}