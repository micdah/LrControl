using System;
using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

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
            ActiveModuleProfile.AssignFunction(controllerId, function);
        }

        public void AssignDevelopFunction(Panel panel, ControllerId controllerId, IFunction function)
        {
            _developModuleProfile.AssignFunction(panel, controllerId, function);
        }

        public void OnControllerInput(ControllerId controllerId, Range range, int value)
        {
            ActiveModuleProfile.OnControllerInput(controllerId, range, value);
        }

        private IModuleProfile ActiveModuleProfile => 
            _activeModule == Module.Develop 
                ? _developModuleProfile 
                : _moduleProfiles[_activeModule];

        private void OnModuleChanged(Module module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            
            _activeModule = module;
        }

        public void Dispose()
        {
            _lrApi.LrApplicationView.ModuleChanged -= OnModuleChanged;
        }
    }

    public interface IModuleProfile
    {
        Module Module { get; }

        void AssignFunction(ControllerId controllerId, IFunction function);

        void OnControllerInput(ControllerId controllerId, Range range, int value);
    }

    public class ModuleProfile : IModuleProfile
    {
        public Module Module { get; }

        public ModuleProfile(Module module)
        {
            Module = module;
        }

        public void AssignFunction(ControllerId controllerId, IFunction function)
        {
            throw new NotImplementedException();
        }
    }

    public class DevelopModuleProfile : IModuleProfile
    {
        public Module Module => Module.Develop;

        public DevelopModuleProfile()
        {
        }

        public void AssignFunction(ControllerId controllerId, IFunction function)
        {
            throw new NotImplementedException();
        }

        public void AssignFunction(Panel panel, ControllerId controllerId, IFunction function)
        {
            throw new NotImplementedException();
        }
    }
}