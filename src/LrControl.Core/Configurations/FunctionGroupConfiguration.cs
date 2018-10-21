using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LrControl.Core.Mapping;

namespace LrControl.Core.Configurations
{
    public class FunctionGroupConfiguration
    {
        [UsedImplicitly]
        public FunctionGroupConfiguration()
        {
        }

        public FunctionGroupConfiguration(ControllerFunctionGroup controllerFunctionGroup)
        {
            Key = controllerFunctionGroup.Key;
            ControllerFunctions = controllerFunctionGroup.ControllerFunctions
                .Select(x => new ControllerFunctionConfiguration(x))
                .ToList();
        }

        public string Key { get; set; }
        public List<ControllerFunctionConfiguration> ControllerFunctions { get; set; }
    }
}