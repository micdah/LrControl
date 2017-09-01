using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LrControl.Api.Common.Attributes;

namespace LrControl.Api.t4
{
    public class ModuleGenerator : GeneratorBase
    {
        private readonly Type _moduleType;
        private LuaNativeModuleAttribute _luaNativeModule;
        

        public ModuleGenerator(Type moduleType)
        {
            _moduleType = moduleType;
        }

        public string GenerateLuaModule()
        {
            Clear();

            GenerateImports();
            Line();

            GenerateDeclarations();

            return Generate();
        }

        private void GenerateImports()
        {
            _luaNativeModule = GetAttribute<LuaNativeModuleAttribute>(_moduleType);
            var pad = PaddingOf(new List<string> { _luaNativeModule.Module, "ModuleTools" }, x => x);

            Line($"local {_luaNativeModule.Module.PadRight(pad)} = import '{_luaNativeModule.Module}'");

            Line($"local {"ModuleTools".PadRight(pad)} = require 'ModuleTools'");
        }

        private void GenerateDeclarations()
        {
            Line("return {");

            using (Indent())
            {
                var methods = GetAllMetohds(_moduleType);
                var pad = PaddingOf(methods, x => x.Name);

                foreach (var info in methods)
                {
                    GenerateMethod(info, pad);
                    Line();
                }
            }

            Line("}");
        }

        private void GenerateMethod(MethodInfo info, int pad)
        {
            var name = char.ToLowerInvariant(info.Name[0]) + info.Name.Substring(1);

            Line($"{name.PadRight(pad)} = ");
            
            using (Indent())
            {
                var requireModule = GetAttribute<LuaRequireModuleAttribute>(info);
                if (requireModule != null)
                {
                    Line($"ModuleTools.RequireModule(\"{requireModule.Module}\", ");
                }

                Line($"ModuleTools.BeforeFunction(\"{_luaNativeModule.Module}.{name}\", ");
                Line($"ModuleTools.AfterFunction(\"{_luaNativeModule.Module}.{name}\", ");

                // Parameters
                var parameters = string.Join(",", info.GetParameters()
                    .Where(p => !p.IsOut)
                    .Select(p => p.Name));

                // Function declaration
                Line($"function({parameters})");

                // Function body
                var hasReturn = info.GetParameters().Any(p => p.IsOut);
                using (Indent())
                {
                    Line();
                    if (hasReturn)
                    {
                        Append("return ");
                    }
                    Append($"{_luaNativeModule.Module}.{name}({parameters})");
                }

                // Function end
                Line("end))");
                if (requireModule != null)
                    Append(")");
                Append(",");
            }
        }
    }
}