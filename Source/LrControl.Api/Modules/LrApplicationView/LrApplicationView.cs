using LrControl.Api.Common;
using LrControl.Api.Communication;

namespace LrControl.Api.Modules.LrApplicationView
{
    internal class LrApplicationView : ModuleBase<LrApplicationView>, ILrApplicationView
    {
        public LrApplicationView(MessageProtocol<LrApplicationView> messageProtocol) : base(messageProtocol)
        {
        }

        public event ModuleChangedHandler ModuleChanged;

        public void OnModuleChanged(string moduleName)
        {
            var module = Module.GetEnumForValue(moduleName);
            if (module != null)
            {
                ModuleChanged?.Invoke(module);
            }
        }

        public bool GetCurrentModuleName(out Module module)
        {
            if (!Invoke(out string result, nameof(GetCurrentModuleName)))
                return False(out module);

            module = Module.GetEnumForValue(result);
            return module != null;
        }

        public bool GetSecondaryViewName(out SecondaryView secondaryView)
        {
            if (!Invoke(out string result, nameof(GetSecondaryViewName)))
                return False(out secondaryView);

            secondaryView = SecondaryView.GetEnumForValue(result);
            return secondaryView != null;
        }

        public bool IsSecondaryDispalyOn(out bool isSecondaryDisplayOn)
        {
            return Invoke(out isSecondaryDisplayOn, nameof(IsSecondaryDispalyOn));
        }

        public bool ShowSecondaryView(SecondaryView view)
        {
            return Invoke(nameof(ShowSecondaryView), view);
        }

        public bool ShowView(PrimaryView view)
        {
            return Invoke(nameof(ShowView), view);
        }

        public bool SwitchToModule(Module module)
        {
            return Invoke(nameof(SwitchToModule), module);
        }

        public bool ToggleSecondaryDisplay()
        {
            return Invoke(nameof(ToggleSecondaryDisplay));
        }

        public bool ToggleSecondaryDisplayFullscreen()
        {
            return Invoke(nameof(ToggleSecondaryDisplayFullscreen));
        }

        public bool ToggleZoom()
        {
            return Invoke(nameof(ToggleZoom));
        }

        public bool ZoomIn()
        {
            return Invoke(nameof(ZoomIn));
        }

        public bool ZoomInSome()
        {
            return Invoke(nameof(ZoomInSome));
        }

        public bool ZoomOut()
        {
            return Invoke(nameof(ZoomOut));
        }

        public bool ZoomOutSome()
        {
            return Invoke(nameof(ZoomOutSome));
        }
    }
}