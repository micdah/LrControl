using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LrControl.LrPlugin.Api.Common
{
    public abstract class Enumeration<TEnum,TValue> : IEnumeration<TValue>
        where TEnum : IEnumeration<TValue>
        where TValue : IComparable 
    {
        private static readonly Lazy<List<TEnum>> AllEnumsCache = new Lazy<List<TEnum>>(() =>
        {
            var fields = typeof (TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(info => info.GetValue(null)).OfType<TEnum>().ToList();
        });

        protected Enumeration(TValue value, string name)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public TValue Value { get; }

        public override string ToString() => Name;

        public static TEnum GetEnumForValue(TValue value)
        {
            return GetAll().FirstOrDefault(e => e.Value.Equals(value));
        }

        public static ICollection<TEnum> GetAll() => AllEnumsCache.Value;
        
        public int CompareTo(object other)
        {
            return Value.CompareTo(((TEnum) other).Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Enumeration<TEnum, TValue>) obj);
        }

        private bool Equals(IEnumeration<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(Value);
        }
    }
}