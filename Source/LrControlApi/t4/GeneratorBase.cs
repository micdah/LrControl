using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using micdah.LrControlApi.Common.Attributes;

namespace micdah.LrControlApi.t4
{
    public class GeneratorBase
    {
        private const int SpacePerIndent = 4;
        private readonly StringBuilder _lua = new StringBuilder();
        private int _indent;

        protected void Line(string line = null)
        {
            _lua.Append('\n');
            if (_indent > 0)
            {
                _lua.Append(new string(' ', SpacePerIndent*_indent));
            }

            _lua.Append(line);
        }

        protected void Clear()
        {
            _lua.Clear();
        }

        protected string Generate()
        {
            return _lua.ToString();
        }

        protected void Append(string text)
        {
            _lua.Append(text);
        }

        protected IDisposable Indent()
        {
            return new IndentClass(this);
        }

        protected List<MethodInfo> GetAllMetohds(Type type)
        {
            return type.GetMethods()
                .Where(info => GetAttribute<MethodAttribute>(info) != null)
                .ToList();
        }

        protected static T GetAttribute<T>(MemberInfo info)
            where T : Attribute
        {
            var attr = Attribute.GetCustomAttribute(info, typeof (T));
            if (attr is T)
            {
                return (T) attr;
            }
            return default(T);
        }

        protected static List<T> GetAttributes<T>(MemberInfo info)
            where T : Attribute
        {
            var attr = Attribute.GetCustomAttributes(info, typeof (T));
            return attr.Cast<T>().ToList();
        }

        protected static int PaddingOf<T>(IEnumerable<T> enumerable, Func<T, string> property)
        {
            return enumerable.Select(x => property(x).Length).Max();
        }

        private class IndentClass : IDisposable
        {
            private readonly GeneratorBase _generator;

            public IndentClass(GeneratorBase generator)
            {
                _generator = generator;
                _generator._indent++;
            }

            public void Dispose()
            {
                _generator._indent--;
            }
        }
    }
}