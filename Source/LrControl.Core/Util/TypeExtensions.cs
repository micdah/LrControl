using System;
using System.Collections.Generic;
using System.Linq;

namespace LrControl.Core.Util
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterface(this Type @type, Type interfaceType)
        {
            IEnumerable<Type> interfaceTypes;
            if (interfaceType.IsGenericType)
            {
                interfaceTypes = @type.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .Select(x => x.GetGenericTypeDefinition());
            }
            else
            {
                interfaceTypes = @type.GetInterfaces();
            }

            return interfaceTypes.Any(x => x == interfaceType);
        }
    }
}