using System;
using System.Collections.Generic;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl
{
    public class ProfileManager : IDisposable
    {
        private readonly LrApi _lrApi;
        private readonly Dictionary<Module, Profile> _moduleProfiles;
        private readonly Dictionary<Panel, Profile> _panelProfiles;

        private Profile _activeModuleProfile;
        private Profile _activePanelProfile;
        
        public ProfileManager(LrApi lrApi)
        {
            _lrApi = lrApi;
            _moduleProfiles = new Dictionary<Module, Profile>();
            _panelProfiles = new Dictionary<Panel, Profile>();
            
            // Initialize profiles
            foreach (var module in Module.GetAll())
            {
                _moduleProfiles[@module] = new Profile();
            }

            foreach (var panel in Panel.GetAll())
            {
                _panelProfiles[panel] = new Profile();
            }
            
            lrApi.LrApplicationView.ModuleChanged += OnModuleChanged;
        }

        private void OnModuleChanged(Module module)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _lrApi.LrApplicationView.ModuleChanged -= OnModuleChanged;
        }
    }
}