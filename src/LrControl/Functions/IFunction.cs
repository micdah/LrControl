using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
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
        /// Apply function using <paramref name="value"/> which is bounded to <paramref name="range"/>
        /// </summary>
        /// <param name="value">Value to use when applying function</param>
        /// <param name="range">Range value must be bounded to</param>
        /// <param name="activeModule">Currently active module</param>
        /// <param name="activePanel">Currently active panel (if any)</param>
        void Apply(int value, Range range, Module activeModule, Panel activePanel);
    }
}