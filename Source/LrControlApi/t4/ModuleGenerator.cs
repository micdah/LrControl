using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using micdah.LrControlApi.Common.Attributes;

namespace micdah.LrControlApi.t4
{
    public class ModuleGenerator : GeneratorBase
    {
        private readonly Type _moduleType;
        private NativeModuleAttribute _nativeModule;
        

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
            _nativeModule = GetAttribute<NativeModuleAttribute>(_moduleType);
            var pad = PaddingOf(new List<string> { _nativeModule.Module, "ModuleTools" }, x => x);

            Line($"local {_nativeModule.Module.PadRight(pad)} = import '{_nativeModule.Module}'");

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

            var requireModule = GetAttribute<RequireModuleAttribute>(info);
            if (requireModule != null)
            {
                Append($"ModuleTools.RequireModule(\"{requireModule.Module}\", ");
            }

            // Parameters
            var parameters = string.Join(",", info.GetParameters()
                .Where(p => !p.IsOut)
                .Select(p => p.Name));
            
            // Function declaration
            Append($"function({parameters})");

            // Function body
            var hasReturn = info.GetParameters().Any(p => p.IsOut);
            using (Indent())
            {
                Line();
                if (hasReturn)
                {
                    Append("return ");
                }
                Append($"{_nativeModule.Module}.{name}({parameters})");
            }

            // Function end
            Line("end");
            if (requireModule != null)
                Append(")");
            Append(",");
        }
    }
}