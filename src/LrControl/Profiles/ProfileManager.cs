using System;
using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    public interface IProfileManager : IDisposable
    {
        Module ActiveModule { get; }
        Panel ActivePanel { get; }
        void AssignFunction(Module module, in ControllerId controllerId, IFunction function);
        void AssignPanelFunction(Panel panel, in ControllerId controllerId, IFunction function);
        void ClearFunction(Module module, in ControllerId controllerId);
        void ClearPanelFunction(Panel panel, in ControllerId controllerId);
        void OnModuleChanged(Module module);
        void OnPanelChanged(Panel panel);
    }

    public class ProfileManager : IProfileManager
    {
        private readonly IDeviceManager _deviceManager;
        private readonly Dictionary<Module, IModuleProfile> _moduleProfiles = new Dictionary<Module, IModuleProfile>();
        private readonly DevelopModuleProfile _developModuleProfile;
        
        public ProfileManager(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            
            // Initialize module profiles
            foreach (var module in Module.GetAll())
            {
                if (module != Module.Develop)
                    _moduleProfiles[module] = new ModuleProfile(module);
            }
            _developModuleProfile = new DevelopModuleProfile();
            
            _deviceManager.Input += OnInput;
        }

        public Module ActiveModule { get; private set; } = Module.Library;

        public Panel ActivePanel
        {
            get => _developModuleProfile.ActivePanel;
            private set => _developModuleProfile.ActivePanel = value;
        }

        public void AssignFunction(Module module, in ControllerId controllerId, IFunction function)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            GetProfileForModule(module).AssignFunction(controllerId, function);
        }

        public void AssignPanelFunction(Panel panel, in ControllerId controllerId, IFunction function)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            
            _developModuleProfile.AssignFunction(panel, controllerId, function);
        }

        public void ClearFunction(Module module, in ControllerId controllerId)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            GetProfileForModule(module).ClearFunction(controllerId);
        }

        public void ClearPanelFunction(Panel panel, in ControllerId controllerId)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));

            _developModuleProfile.ClearFunction(panel, controllerId);
        }

        public void OnModuleChanged(Module module)
        {
            ActiveModule = module;

            ResetControllersWithoutFunction();
        }

        public void OnPanelChanged(Panel panel)
        {
            ActivePanel = panel;
            
            ResetControllersWithoutFunction();
        }

        public void Dispose()
        {
            _deviceManager.Input -= OnInput;
        }

        private void ResetControllersWithoutFunction()
        {
            var activeProfile = GetProfileForModule(ActiveModule);
            foreach (var info in _deviceManager.ControllerInfos)
            {
                if (!activeProfile.HasFunction(info.ControllerId))
                {
                    _deviceManager.OnOutput(info.ControllerId, (int) info.Range.Minimum);
                }
            }
        }

        private void OnInput(in ControllerId controllerId, Range range, int value)
        {
            GetProfileForModule(ActiveModule).ApplyFunction(controllerId, value, range, ActiveModule, ActivePanel);
        }

        private IModuleProfile GetProfileForModule(Module module)
            => module == Module.Develop
                ? _developModuleProfile
                : _moduleProfiles[module];
    }
}