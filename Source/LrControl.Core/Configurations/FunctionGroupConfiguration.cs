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

        public FunctionGroupConfiguration(FunctionGroup functionGroup)
        {
            Key = functionGroup.Key;
            ControllerFunctions = functionGroup.ControllerFunctions
                .Select(x => new ControllerFunctionConfiguration(x))
                .ToList();
        }

        public string Key { get; set; }
        public List<ControllerFunctionConfiguration> ControllerFunctions { get; set; }
    }
}