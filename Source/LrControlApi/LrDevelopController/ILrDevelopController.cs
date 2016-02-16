using System.Text;
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
        bool Decrement(IDevelopControllerParameter param);

        /// <summary>
        ///     Returns the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <returns></returns>
        bool GetProcessVersion(out ProcessVersion processVersion);

        /// <summary>
        ///     Gets the min and max value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetRange(out Range range, IDevelopControllerParameter param);

        /// <summary>
        ///     Reports which tool mode is active in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        bool GetSelectedTool(out Tool tool);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetValue(out string value, IDevelopControllerParameter param);

        /// <summary>
        ///     Increments the value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        bool Increment(IDevelopControllerParameter param);

        /// <summary>
        ///     Resets all Develop adjustments for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetAllDevelopAdjustments();

        /// <summary>
        ///     Clears all localized adjustment brushing from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetBrushing();

        /// <summary>
        ///     lears all radial filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetCircularGradient();

        /// <summary>
        ///     Resets the crop angle and frame for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetCrop();

        /// <summary>
        ///     Clears all graduated filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetGradient();

        /// <summary>
        ///     Clears all redeye removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetRedEye();

        /// <summary>
        ///     Clears all spot removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool ResetSpotRemoval();

        /// <summary>
        ///     Resets a single Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        bool ResetToDefault(IDevelopControllerParameter param);

        /// <summary>
        ///     Enables a mode where adjusting a parameter causes that panel to be automatically revealed in the panel track.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="reveal"></param>
        bool RevealAdjustedControls(bool reveal);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        bool RevealPanel(IDevelopControllerParameter param);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="panel"></param>
        bool Revealpanel(Panel panel);

        /// <summary>
        ///     Select a tool mode in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="tool"></param>
        bool SelectTool(Tool tool);

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
        bool SetMultipleAdjustmentThreshold(double seconds);

        /// <summary>
        ///     Sets the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="version"></param>
        bool SetProcessVersion(ProcessVersion version);

        /// <summary>
        ///     Sets the number of seconds that tracking remains enabled after each adjustment is made. The default is 2 seconds.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="seconds"></param>
        bool SetTrackingDelay(double seconds);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IDevelopControllerParameter param, string value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IDevelopControllerParameter param, double value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IDevelopControllerParameter param, int value);

        /// <summary>
        ///     Temporarily puts the Develop module into its tracking state, causing faster, lower-quailty redraw and preventing
        ///     history states from being generated. Tracking will automatically be turned back off as soon as a diferent parameter
        ///     is adjusted, or two seconds after the last adjustment is made.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        bool StartTracking(IDevelopControllerParameter param);

        /// <summary>
        ///     Causes Develop module to exit its tracking state immediately, creating a single history state for all changes that
        ///     were made to the parameter that was being tracked.
        ///     Must be called while the Develop module is active.
        /// </summary>
        bool StopTracking();
    }
}