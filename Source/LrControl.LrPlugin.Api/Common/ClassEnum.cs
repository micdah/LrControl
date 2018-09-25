using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.LrPlugin.Api.Common
{
    public abstract class ClassEnum<TValue, TEnum> : IClassEnum<TValue>
        where TEnum : ClassEnum<TValue, TEnum>
    {
        private static readonly Lazy<List<TEnum>> AllEnumsCache = new Lazy<List<TEnum>>(GetAllEnums);

        protected ClassEnum(TValue value, string name)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public TValue Value { get; }

        public override string ToString()
        {
            return Name;
        }

        public static void CallSetValue(LrApi api, IParameter<TEnum> parameter, TEnum value)
        {
            api.LrDevelopController.SetValue(parameter, value);
        }

        private static List<TEnum> GetAllEnums()
        {
            var fields = typeof (TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(info => info.GetValue(null)).OfType<TEnum>().ToList();
        }

        public static TEnum GetEnumForValue(TValue value)
        {
            return AllEnumsCache.Value.FirstOrDefault(e => e.Value.Equals(value));
        }

        public static IList<TEnum> AllEnums => AllEnumsCache.Value.AsReadOnly();
    }
}