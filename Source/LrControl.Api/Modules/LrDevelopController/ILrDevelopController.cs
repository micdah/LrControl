using LrControl.Api.Common;
using LrControl.Api.Common.Attributes;

namespace LrControl.Api.Modules.LrDevelopController
{
    public delegate void ParameterChangedHandler();

    [LuaNativeModule("LrDevelopController")]
    public interface ILrDevelopController
    {
        void AddParameterChangedListener(IParameter parameter, ParameterChangedHandler handler);
        void RemoveParameterChangedListener(IParameter parameter, ParameterChangedHandler handler);

        /// <summary>
        ///     Increments the value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool Decrement(IParameter param);

        /// <summary>
        ///     Returns the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <returns></returns>
        [LuaMethod, LuaRequireModule("develop")]
        bool GetProcessVersion(out ProcessVersion processVersion);

        /// <summary>
        ///     Gets the min and max value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [LuaMethod, LuaRequireModule("develop")]
        bool GetRange(out Range range, IParameter param);

        /// <summary>
        ///     Reports which tool mode is active in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        [LuaMethod, LuaRequireModule("develop")]
        bool GetSelectedTool(out Tool tool);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [LuaMethod, LuaRequireModule("develop")]
        bool GetValue(out int value, IParameter<int> param);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetValue(out double value, IParameter<double> param);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetValue(out bool value, IParameter<bool> param);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetValue(out string value, IParameter<string> param);

        /// <summary>
        ///     Gets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool GetValue<TEnum, TValue>(out TEnum value, IParameter<ClassEnum<TValue,TEnum>> param)
            where TEnum : ClassEnum<TValue, TEnum>;

        /// <summary>
        ///     Increments the value of a Develop adjustment.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool Increment(IParameter param);

        /// <summary>
        ///     Resets all Develop adjustments for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetAllDevelopAdjustments();

        /// <summary>
        ///     Clears all localized adjustment brushing from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetBrushing();

        /// <summary>
        ///     Clears all radial filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetCircularGradient();

        /// <summary>
        ///     Resets the crop angle and frame for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetCrop();

        /// <summary>
        ///     Clears all graduated filter adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetGradient();

        /// <summary>
        ///     Clears all redeye removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetRedEye();

        /// <summary>
        ///     Clears all spot removal adjustments from the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetSpotRemoval();

        /// <summary>
        ///     Resets a single Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool ResetToDefault(IParameter param);

        /// <summary>
        ///     Enables a mode where adjusting a parameter causes that panel to be automatically revealed in the panel track.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="reveal"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool RevealAdjustedControls(bool reveal);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool RevealPanel(IParameter param);

        /// <summary>
        ///     Expands and scrolls into view the panel with the given ID.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="panel"></param>
        [LuaRequireModule("develop")]
        bool RevealPanel(Panel panel);

        /// <summary>
        ///     Select a tool mode in Develop.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="tool"></param>
        [LuaMethod, LuaRequireModule("develop")]
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
        [LuaMethod, LuaRequireModule("develop")]
        bool SetMultipleAdjustmentThreshold(double seconds);

        /// <summary>
        ///     Sets the process version of the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="version"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool SetProcessVersion(ProcessVersion version);

        /// <summary>
        ///     Sets the number of seconds that tracking remains enabled after each adjustment is made. The default is 2 seconds.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="seconds"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool SetTrackingDelay(double seconds);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool SetValue(IParameter<int> param, int value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IParameter<double> param, double value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IParameter<bool> param, bool value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue(IParameter<string> param, string value);

        /// <summary>
        ///     Sets the value of a Develop adjustment for the current photo.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        bool SetValue<TEnum, TValue>(IParameter<TEnum> param, ClassEnum<TValue,TEnum> value)
            where TEnum : ClassEnum<TValue,TEnum>;

        /// <summary>
        ///     Temporarily puts the Develop module into its tracking state, causing faster, lower-quailty redraw and preventing
        ///     history states from being generated. Tracking will automatically be turned back off as soon as a diferent parameter
        ///     is adjusted, or two seconds after the last adjustment is made.
        ///     Must be called while the Develop module is active.
        /// </summary>
        /// <param name="param"></param>
        [LuaMethod, LuaRequireModule("develop")]
        bool StartTracking(IParameter param);

        /// <summary>
        ///     Causes Develop module to exit its tracking state immediately, creating a single history state for all changes that
        ///     were made to the parameter that was being tracked.
        ///     Must be called while the Develop module is active.
        /// </summary>
        [LuaMethod, LuaRequireModule("develop")]
        bool StopTracking();
    }
}