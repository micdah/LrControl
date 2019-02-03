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
        private Module _activeModule = Module.Web;
        
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

        public void AssignFunction(Module module, ControllerId controllerId, IFunction function)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            
            ActiveModuleProfile.AssignFunction(controllerId, function);
        }

        public void AssignDevelopFunction(Panel panel, ControllerId controllerId, IFunction function)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            
            _developModuleProfile.AssignFunction(panel, controllerId, function);
        }

        public void ClearFunction(Module module, ControllerId controllerId)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            ActiveModuleProfile.ClearFunction(controllerId);
        }

        public void ClearDevelopFunction(Panel panel, ControllerId controllerId)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));

            _developModuleProfile.ClearFunction(panel, controllerId);
        }

        public void OnControllerInput(ControllerId controllerId, Range range, int value)
        {
            ActiveModuleProfile.OnControllerInput(controllerId, value, range);
        }

        private IModuleProfile ActiveModuleProfile => 
            _activeModule == Module.Develop 
                ? _developModuleProfile 
                : _moduleProfiles[_activeModule];

        private void OnModuleChanged(Module module)
        {
            _activeModule = module;
        }

        public void Dispose()
        {
            _lrApi.LrApplicationView.ModuleChanged -= OnModuleChanged;
        }
    }
}