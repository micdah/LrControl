using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LrControl.Api.Modules.LrDevelopController;

namespace LrControl.Api.Common
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
            var all = new List<TEnum>();

            var fields = typeof (TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var info in fields)
            {
                var value = info.GetValue(null) as TEnum;
                if (value != null)
                {
                    all.Add(value);
                }
            }

            return all;
        }

        public static TEnum GetEnumForValue(TValue value)
        {
            return AllEnumsCache.Value.FirstOrDefault(e => e.Value.Equals(value));
        }

        public static IList<TEnum> AllEnums => AllEnumsCache.Value.AsReadOnly();
    }
}