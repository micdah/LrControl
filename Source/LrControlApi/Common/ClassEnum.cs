using System.Collections.Generic;
using System.Linq;
using log4net;

namespace micdah.LrControlApi.Common
{
    public abstract class ClassEnum<TValue, TEnum> : IClassEnum<TValue>
        where TEnum : ClassEnum<TValue, TEnum>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ClassEnum<TValue, TEnum>));
        private static readonly List<TEnum> AllEnumsLookup = new List<TEnum>();

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

        public static TEnum GetEnumForValue(TValue value)
        {
            return AllEnumsLookup.FirstOrDefault(e => e.Value.Equals(value));
        }

        protected static void AddEnums(params TEnum[] enums)
        {
            AllEnumsLookup.AddRange(enums);
        }
    }
}