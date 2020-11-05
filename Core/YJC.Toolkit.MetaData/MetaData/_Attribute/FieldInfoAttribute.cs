using System;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FieldInfoAttribute : Attribute
    {
        public FieldInfoAttribute()
        {
            IsEmpty = true;
        }

        public HintPosition HintPosition { get; set; }

        public string Hint { get; set; }

        public int Length { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsKey { get; set; }
    }
}
