namespace LrControlApi.Modules.LrUndo
{
    public interface ILrUndo
    {
        /// <summary>
        ///     Returns true of the redo command is currently enabled.
        /// </summary>
        /// <returns></returns>
        bool CanRedo();

        /// <summary>
        ///     Returns true of the undo command is currently enabled.
        /// </summary>
        /// <returns></returns>
        bool CanUndo();

        /// <summary>
        ///     Redoes the last undone history state.
        /// </summary>
        void Redo();

        /// <summary>
        ///     Undoes the last history state.
        /// </summary>
        void Undo();
    }
}