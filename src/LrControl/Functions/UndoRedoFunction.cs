using LrControl.Configurations;
using LrControl.Enums;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class UndoRedoFunction : Function
    {
        private readonly Operation _operation;

        public UndoRedoFunction(ISettings settings, ILrApi api, string displayName, string key, Operation operation) 
            : base(settings, api, displayName, key)
        {
            _operation = operation;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;

            switch (_operation)
            {
                case Operation.Undo:
                    if (Api.LrUndo.CanUndo(out var canUndo) && canUndo)
                    {
                        Api.LrDevelopController.StopTracking();
                        Api.LrUndo.Undo();
                    }
                    break;
                case Operation.Redo:
                    if (Api.LrUndo.CanRedo(out var canRedo) && canRedo)
                    {
                        Api.LrDevelopController.StopTracking();
                        Api.LrUndo.Redo();
                    }
                    break;
            }
        }
    }
}