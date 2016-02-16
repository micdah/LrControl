using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace micdah.LrControlApi.Common
{
    public abstract class ClassEnum<TValue, TEnum> : IClassEnum<TValue>
        where TEnum : ClassEnum<TValue, TEnum>
    {
        private static readonly Lazy<IList<TEnum>> AllEnums = new Lazy<IList<TEnum>>(GetAllEnums);

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

        private static IList<TEnum> GetAllEnums()
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
            return AllEnums.Value.FirstOrDefault(e => e.Value.Equals(value));
        }
    }
}