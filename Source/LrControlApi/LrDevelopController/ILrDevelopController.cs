using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public delegate void AdjustmentChange();

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
        ProcessVersion GetProcessVersion();

        /// <summary>
        ///     Gets the min and max value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Range GetRange(IDevelopControllerParameter param);

        /// <summary>
        ///     Reports which tool mode is active in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <returns></returns>
        Tool GetSelectedTool();

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        object GetValue(IDevelopControllerParameter param);

        /// <summary>
        ///     Increments the value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void Increment(IDevelopControllerParameter param);

        /// <summary>
        ///     Resets all Develop adjustments for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetAllDevelopAdjustments();

        /// <summary>
        ///     Clears all localized adjustment brushing from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetBrushing();

        /// <summary>
        ///     lears all radial filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetCircularGradient();

        /// <summary>
        ///     Resets the crop angle and frame for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetCrop();

        /// <summary>
        ///     Clears all graduated filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetGradient();

        /// <summary>
        ///     Clears all redeye removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetRedEye();

        /// <summary>
        ///     Clears all spot removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void ResetSpotRemoval();

        /// <summary>
        ///     Resets a single Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void ResetToDefault(IDevelopControllerParameter param);

        /// <summary>
        ///     Enables a mode where adjusting a parameter causes that panel to be automatically revealed in the panel track.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="reveal"></param>
        void RevealAdjustedControls(bool reveal);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void RevealPanel(IDevelopControllerParameter param);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="panel"></param>
        void Revealpanel(Panel panel);

        /// <summary>
        ///     Select a tool mode in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="tool"></param>
        void SelectTool(Tool tool);

        /// <summary>
        ///     Sets the time threshold that determines when adjustments to different parameters will be grouped together into a
        ///     single history state versus recorded separately. If multiple different parameters are changed within a window of
        ///     time that is less than this threshold, they will be grouped together into a single "Multiple Settings" history
        ///     state. If they occur farther apart than this threshold, each will get its own history state. Recording many
        ///     separate history states in rapid succession can degrade Lightroom's performance, so this threshold is very
        ///     important if simultaneous adjustments by the user are a possibility. The default is 0.5 seconds. Has no effect if
        ///     set to a value that is higher than the tracking delay.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="seconds"></param>
        void SetMultipleAdjustmentThreshold(double seconds);

        /// <summary>
        ///     Sets the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="version"></param>
        void SetProcessVersion(ProcessVersion version);

        /// <summary>
        ///     Sets the number of seconds that tracking remains enabled after each adjustment is made. The default is 2 seconds.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="seconds"></param>
        void SetTrackingDelay(double seconds);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        void SetValue(IDevelopControllerParameter param, object value);

        /// <summary>
        ///     Temporarily puts the Develop module into its tracking state, causing faster, lower-quailty redraw and preventing
        ///     history states from being generated. Tracking will automatically be turned back off as soon as a diferent parameter
        ///     is adjusted, or two seconds after the last adjustment is made.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        void StartTracking(IDevelopControllerParameter param);

        /// <summary>
        ///     Causes Develop module to exit its tracking state immediately, creating a single history state for all changes that
        ///     were made to the parameter that was being tracked.
        ///     Must be called while the Develop module is active.
        /// </summary>
        void StopTracking();
    }
}