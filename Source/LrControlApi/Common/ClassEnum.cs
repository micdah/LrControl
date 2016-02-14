using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LrControlApi.Common
{
    public abstract class ClassEnum<TValue, TEnum> : ClassEnum
        where TEnum : ClassEnum<TValue, TEnum>
    {
        private static readonly List<TEnum> AllEnumsLookup = new List<TEnum>();

        protected ClassEnum(TValue value, string name)
        {
            Name = name;
            Value = value;

            AllEnumsLookup.Add((TEnum) this);
        }

        public string Name { get; }
        public TValue Value { get; }

        public override object ObjectValue => Value;

        public static IList<TEnum> AllValues => new ReadOnlyCollection<TEnum>(AllEnumsLookup);

        public static TEnum GetEnumForValue(TValue value)
        {
            return AllValues.FirstOrDefault(e => e.Value.Equals(value));
        }  

        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class ClassEnum
    {
        public abstract object ObjectValue { get; }
    }
}