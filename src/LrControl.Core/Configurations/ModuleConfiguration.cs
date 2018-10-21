using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LrControl.Core.Mapping;

namespace LrControl.Core.Configurations
{
    public class ModuleConfiguration
    {
        [UsedImplicitly]
        public ModuleConfiguration()
        {
        }

        public ModuleConfiguration(ModuleGroup moduleGroup)
        {
            ModuleName = moduleGroup.Module.Value;
            FunctionGroups = moduleGroup.ControllerFunctionGroups
                .Select(x => new FunctionGroupConfiguration(x))
                .ToList();
        }

        public string ModuleName { get; set; }
        public List<FunctionGroupConfiguration> FunctionGroups { get; set; }
    }
}