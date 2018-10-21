using System;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Core.Functions
{
    public interface IFunction
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
    }
}