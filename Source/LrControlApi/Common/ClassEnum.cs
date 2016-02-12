using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControlApi.Common
{
    public abstract class ClassEnum<TValue,TEnum>
        where TEnum : ClassEnum<TValue,TEnum>
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

        public override string ToString()
        {
            return Name;
        }

        public static IList<TEnum> Values => new ReadOnlyCollection<TEnum>(AllEnumsLookup);
    }
}