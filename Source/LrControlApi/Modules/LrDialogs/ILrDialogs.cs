namespace LrControlApi.Modules.LrDialogs
{
    public interface ILrDialogs
    {
        /// <summary>
        ///     Invokes a modal dialog for confirmation. Displays a message, with an action button, a cancel button, and optionally
        ///     one other button.
        /// </summary>
        /// <param name="message">The main alert message string, the title of the dialog.</param>
        /// <param name="info">
        ///     A secondary message string, shown in a smaller font below the main message. Usually more
        ///     descriptive. Can be nil.
        /// </param>
        /// <param name="actionVerb">The label string for the action button. Default is "OK".</param>
        /// <param name="cancelVerb">The label string for the cancel button. Default is "Cancel".</param>
        /// <param name="otherVerb">A label string for an optional third button. If not supplied, no third button is shown.</param>
        /// <returns>The button used to dismiss the dialog, one of "ok", "cancel" or "other"</returns>
        ConfirmResult Confirm(string message, string info = null, string actionVerb = "OK", string cancelVerb = "Cancel",
            string otherVerb = null);

        /// <summary>
        ///     Invokes a modal dialog to display a message, with a single "OK" button that dismisses the dialog.
        /// </summary>
        /// <param name="message">The main message to display.</param>
        /// <param name="info">A secondary message to display, shown in a smaller font below the main message. Can be nil.</param>
        /// <param name="style">
        ///     The visual style of the dialog, one of: "warning" (the default), "info", or "critical". In Mac OS,
        ///     this affects the style of the icon.
        /// </param>
        void Message(string message, string info = null, DialogStyle style = null);

        /// <summary>
        ///     Shows a message in a window that quickly fades away.
        ///     First supported in version 5.0 of the Lightroom SDK.
        ///     Only one bezel will be visible at any given time.The latest call to LrDialogs.showBezel overrides any existing
        ///     bezel.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="fadeDelay">The number of seconds to show the message. Default is 1.2 seconds.</param>
        void ShowBezel(string message, double? fadeDelay = null);

        /// <summary>
        ///     Invokes a modal dialog that displays an error message.
        /// </summary>
        /// <param name="errorString">
        ///     The error string to display. If this string is a valid error string (i.e. from LrErrors) then
        ///     that error will be displayed. If the string is not a valid error string then a generic error message will be
        ///     displayed and the error string will be used as secondary information.
        /// </param>
        void ShowError(string errorString);
    }
}