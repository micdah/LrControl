using System;
using System.Collections.Generic;
using System.Linq;

namespace LrControl.Core.Util
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if <paramref name="@type"/> is a type of <paramref name="typeOf"/> of implements it as an interface
        /// </summary>
        /// <remarks>
        /// Also works for generic type, i.e. <c>typeof(List&lt;string&gt;).IsTypeOf(typeof(IList&lt;&gt;))</c> is true
        /// </remarks>
        /// <param name="type">Type to check</param>
        /// <param name="typeOf">Type to check against</param>
        /// <returns>True if <paramref name="type"/> is a type of <paramref name="typeOf"/></returns>
        public static bool IsTypeOf(this Type @type, Type typeOf)
        {
            if (@type == typeOf) return true;
            if (typeOf.IsAssignableFrom(@type)) return true;

            if (typeOf.IsInterface)
            {
                IEnumerable<Type> interfaceTypes;
                
                if (typeOf.ContainsGenericParameters)
                {
                    interfaceTypes = @type.GetInterfaces()
                        .Select(t => t.IsGenericType 
                            ? t.GetGenericTypeDefinition()
                            : t);
                }
                else
                {
                    interfaceTypes = @type.GetInterfaces();
                }

                if (interfaceTypes.Contains(typeOf))
                    return true;
            }
            else
            {
                var baseType = @type;
                while (baseType != null)
                {
                    var checkType = baseType.IsGenericType && typeOf.ContainsGenericParameters
                        ? baseType.GetGenericTypeDefinition()
                        : baseType;

                    if (checkType == typeOf)
                        return true;
                    
                    baseType = baseType.BaseType;
                }
            }

            return false;
        }
    }
}