using System;
using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    public interface IProfileManager
    {
        Module ActiveModule { get; set; }
        Panel ActivePanel { get; set; }
        void AssignFunction(Module module, in ControllerId controllerId, IFunction function);
        void AssignPanelFunction(Panel panel, in ControllerId controllerId, IFunction function);
        void ClearFunction(Module module, in ControllerId controllerId);
        void ClearPanelFunction(Panel panel, in ControllerId controllerId);
        void OnModuleChanged(Module module);
        void OnPanelChanged(Panel panel);
        void OnControllerInput(in ControllerId controllerId, Range range, int value);
    }

    public class ProfileManager : IProfileManager
    {
        private readonly Dictionary<Module, IModuleProfile> _moduleProfiles = new Dictionary<Module, IModuleProfile>();
        private readonly DevelopModuleProfile _developModuleProfile;
        
        public ProfileManager()
        {
            // Initialize module profiles
            foreach (var module in Module.GetAll())
            {
                if (module != Module.Develop)
                    _moduleProfiles[module] = new ModuleProfile(module);
            }
            _developModuleProfile = new DevelopModuleProfile();
        }

        public Module ActiveModule { get; set; } = Module.Library;

        public Panel ActivePanel
        {
            get => _developModuleProfile.ActivePanel;
            set => _developModuleProfile.ActivePanel = value;
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
        }

        public void OnPanelChanged(Panel panel)
        {
            throw new NotImplementedException();
        }

        public void OnControllerInput(in ControllerId controllerId, Range range, int value)
        {
            GetProfileForModule(ActiveModule).OnControllerInput(controllerId, value, range);
        }

        private IModuleProfile GetProfileForModule(Module module)
            => module == Module.Develop
                ? _developModuleProfile
                : _moduleProfiles[module];
    }
}