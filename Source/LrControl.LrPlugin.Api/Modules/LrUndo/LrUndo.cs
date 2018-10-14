using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Communication;

namespace LrControl.LrPlugin.Api.Modules.LrUndo
{
    internal class LrUndo : ModuleBase, ILrUndo
    {
        public LrUndo(MessageProtocol messageProtocol) : base(messageProtocol)
        {
        }

        public bool CanRedo(out bool canRedo)
        {
            return Invoke(out canRedo, nameof(CanRedo));
        }

        public bool CanUndo(out bool canUndo)
        {
            return Invoke(out canUndo, nameof(CanUndo));
        }

        public bool Redo()
        {
            return Invoke(nameof(Redo));
        }

        public bool Undo()
        {
            return Invoke(nameof(Undo));
        }
    }
}