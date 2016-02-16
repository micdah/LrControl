namespace micdah.LrControlApi.Modules.LrApplicationView
{
    public delegate void ModuleChangedHandler(Module newModule);

    public interface ILrApplicationView
    {
        event ModuleChangedHandler ModuleChanged;

        /// <summary>
        ///     Returns the name of the currently active module.
        /// </summary>
        /// <returns></returns>
        Module GetCurrentModuleName();

        /// <summary>
        ///     Returns the name of the view currently showing on the secondary screen, or nil of the secondary display is not on.
        /// </summary>
        /// <returns></returns>
        SecondaryView GetSecondaryViewName();

        /// <summary>
        ///     Returns true if the secondary window is currently on.
        /// </summary>
        /// <returns></returns>
        bool IsSecondaryDispalyOn();

        /// <summary>
        ///     Shows a view on the secondary screen, or hides the secondary screen if the given view was previously being shown.
        /// </summary>
        /// <param name="view"></param>
        void ShowSecondaryView(SecondaryView view);

        /// <summary>
        ///     Switches the app's view mode.
        /// </summary>
        /// <param name="view"></param>
        void ShowView(PrimaryView view);

        /// <summary>
        ///     Switches between modules.
        /// </summary>
        /// <param name="module"></param>
        void SwitchToModule(Module module);

        /// <summary>
        ///     Toggles the the secondary window on/off.
        /// </summary>
        void ToggleSecondaryDisplay();

        /// <summary>
        ///     Toggles fullscreen mode for the secondary window.
        /// </summary>
        void ToggleSecondaryDisplayFullscreen();

        /// <summary>
        ///     Zooms toggles between zoomed in and zoomed out. Only works in Library and Develop modules.
        /// </summary>
        void ToggleZoom();

        /// <summary>
        ///     Zooms in one large step. Only works in Library and Develop modules.
        /// </summary>
        void ZoomIn();

        /// <summary>
        ///     Zooms in one small step. Only works in Library and Develop modules.
        /// </summary>
        void ZoomInSome();

        /// <summary>
        ///     Zooms out one large step. Only works in Library and Develop modules.
        /// </summary>
        void ZoomOut();

        /// <summary>
        ///     Zooms out one small step. Only works in Library and Develop modules.
        /// </summary>
        void ZoomOutSome();
    }
}