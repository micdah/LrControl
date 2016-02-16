using micdah.LrControlApi.Common.Attributes;

namespace micdah.LrControlApi.Modules.LrApplicationView
{
    public delegate void ModuleChangedHandler(Module newModule);

    [LuaNativeModule("LrApplicationView")]
    public interface ILrApplicationView
    {
        event ModuleChangedHandler ModuleChanged;

        /// <summary>
        ///     Returns the name of the currently active module.
        /// </summary>
        /// <returns></returns>
        [LuaMethod]
        bool GetCurrentModuleName(out Module module);

        /// <summary>
        ///     Returns the name of the view currently showing on the secondary screen, or nil of the secondary display is not on.
        /// </summary>
        /// <returns></returns>
        [LuaMethod]
        bool GetSecondaryViewName(out SecondaryView secondaryView);

        /// <summary>
        ///     Returns true if the secondary window is currently on.
        /// </summary>
        /// <returns></returns>
        [LuaMethod]
        bool IsSecondaryDispalyOn(out bool isSecondaryDisplayOn);

        /// <summary>
        ///     Shows a view on the secondary screen, or hides the secondary screen if the given view was previously being shown.
        /// </summary>
        /// <param name="view"></param>
        [LuaMethod]
        bool ShowSecondaryView(SecondaryView view);

        /// <summary>
        ///     Switches the app's view mode.
        /// </summary>
        /// <param name="view"></param>
        [LuaMethod]
        bool ShowView(PrimaryView view);

        /// <summary>
        ///     Switches between modules.
        /// </summary>
        /// <param name="module"></param>
        [LuaMethod]
        bool SwitchToModule(Module module);

        /// <summary>
        ///     Toggles the the secondary window on/off.
        /// </summary>
        [LuaMethod]
        bool ToggleSecondaryDisplay();

        /// <summary>
        ///     Toggles fullscreen mode for the secondary window.
        /// </summary>
        [LuaMethod]
        bool ToggleSecondaryDisplayFullscreen();

        /// <summary>
        ///     Zooms toggles between zoomed in and zoomed out. Only works in Library and Develop modules.
        /// </summary>
        [LuaMethod, LuaRequireModule("library,develop")]
        bool ToggleZoom();

        /// <summary>
        ///     Zooms in one large step. Only works in Library and Develop modules.
        /// </summary>
        [LuaMethod, LuaRequireModule("library,develop")]
        bool ZoomIn();

        /// <summary>
        ///     Zooms in one small step. Only works in Library and Develop modules.
        /// </summary>
        [LuaMethod, LuaRequireModule("library,develop")]
        bool ZoomInSome();

        /// <summary>
        ///     Zooms out one large step. Only works in Library and Develop modules.
        /// </summary>
        [LuaMethod, LuaRequireModule("library,develop")]
        bool ZoomOut();

        /// <summary>
        ///     Zooms out one small step. Only works in Library and Develop modules.
        /// </summary>
        [LuaMethod, LuaRequireModule("library,develop")]
        bool ZoomOutSome();
    }
}