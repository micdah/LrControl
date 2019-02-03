using LrControl.Core.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Factories
{
    internal class UndoRedoFunctionFactory : FunctionFactory
    {
        public UndoRedoFunction.Operation Operation { get; }

        public UndoRedoFunctionFactory(ISettings settings, ILrApi api, UndoRedoFunction.Operation operation) 
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