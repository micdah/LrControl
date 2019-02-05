using System;
using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Profiles;

namespace LrControl
{
    public class ProfileManager : IDisposable
    {
        private readonly ILrApi _lrApi;
        private readonly Dictionary<Module, IModuleProfile> _moduleProfiles = new Dictionary<Module, IModuleProfile>();
        private readonly DevelopModuleProfile _developModuleProfile;
        
        public ProfileManager(ILrApi lrApi)
        {
            _lrApi = lrApi;

            // Initialize module profiles
            foreach (var module in Module.GetAll())
            {
                if (module != Module.Develop)
                    _moduleProfiles[module] = new ModuleProfile(module);
            }
            _developModuleProfile = new DevelopModuleProfile();
            
            lrApi.LrApplicationView.ModuleChanged += OnModuleChanged;
        }

        public Module ActiveModule { get; set; } = Module.Web;

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
            
            ActiveModuleProfile.AssignFunction(controllerId, function);
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

            ActiveModuleProfile.ClearFunction(controllerId);
        }

        public void ClearPanelFunction(Panel panel, in ControllerId controllerId)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));

            _developModuleProfile.ClearFunction(panel, controllerId);
        }

        public void OnControllerInput(in ControllerId controllerId, Range range, int value)
        {
            ActiveModuleProfile.OnControllerInput(controllerId, value, range);
        }

        private IModuleProfile ActiveModuleProfile => 
            ActiveModule == Module.Develop 
                ? _developModuleProfile 
                : _moduleProfiles[ActiveModule];

        private void OnModuleChanged(Module module)
        {
            ActiveModule = module;
        }

        public void Dispose()
        {
            _lrApi.LrApplicationView.ModuleChanged -= OnModuleChanged;
        }
    }
}