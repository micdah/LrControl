using System;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Core.Functions
{
    public interface IFunction : IDisposable
    {
        /// <summary>
        /// Unique key used for saving configuration
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Display name shown to user
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Controller value has changed, apply function
        /// </summary>
        /// <param name="controllerValue">Controller value</param>
        /// <param name="controllerRange">Range of controller value</param>
        void ControllerValueChanged(int controllerValue, Range controllerRange);

        /// <summary>
        /// Update controller value, based on function
        /// </summary>
        /// <param name="controllerValue">Controller value to set</param>
        /// <param name="controllerRange">Range of controller value</param>
        /// <returns>True if function has a value to assign the controller value, false otherwise</returns>
        bool UpdateControllerValue(out int controllerValue, Range controllerRange);

        /// <summary>
        /// Can be invoked, to request updating the controller value using <see cref="UpdateControllerValue"/>,
        /// will only be invoked if the controller can be updated as this time.
        /// </summary>
        Action OnRequestUpdateControllerValue { set; }
    }
}