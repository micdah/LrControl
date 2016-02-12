namespace LrControlProxy.Common
{
    public abstract class ClassEnum<TValue>
    {
        protected ClassEnum(string name, TValue value)
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
    }
}