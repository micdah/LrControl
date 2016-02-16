using micdah.LrControlApi.Common;
using micdah.LrControlApi.Communication;

namespace micdah.LrControlApi.Modules.LrUndo
{
    internal class LrUndo : ModuleBase<LrUndo>, ILrUndo
    {
        public LrUndo(MessageProtocol<LrUndo> messageProtocol) : base(messageProtocol)
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