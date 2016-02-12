using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public delegate void AdjustmentChange(string parameter, int newValue);

    public interface ILrDevelopController
    {
        event AdjustmentChange AdjustmentChangeObserver;

        /// <summary>
        ///     Increments the value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void Decrement(IDevelopControllerParameter param);

        /// <summary>
        ///     Returns the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <returns></returns>
        string GetProcessVersion();

        /// <summary>
        ///     Gets the min and max value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Range GetRange(IDevelopControllerParameter param);

        /// <summary>
        ///    Reports which tool mode is active in Develop.
        /// Must be called while the Develop module is active.
        /// </summary>
        /// <returns></returns>
        Tool GetSelectedTool();

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        /// Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        object GetValue(IDevelopControllerParameter param);

        /// <summary>
        /// Increments the value of a Develop adjustment.
        /// Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void Increment(IDevelopControllerParameter param);

        /// <summary>
        ///     Resets all Develop adjustments for the current photo.
        /// Must be called while the Develop module is active.
        /// </summary>
        void ResetAllDevelopAdjustments();

        /// <summary>
        ///     Clears all localized adjustment brushing from the current photo.
        /// Must be called while the Develop module is active.
        /// </summary>
        void ResetBrushing();

        /// <summary>
        ///     lears all radial filter adjustments from the current photo.
        /// Must be called while the Develop module is active.
        /// </summary>
        void ResetCircularGradient();

        /// <summary>
        ///     Resets the crop angle and frame for the current photo.
        /// Must be called while the Develop module is active.
        /// </summary>
        void ResetCrop();
    }
}