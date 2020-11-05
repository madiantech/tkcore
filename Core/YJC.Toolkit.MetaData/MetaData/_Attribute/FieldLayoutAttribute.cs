using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FieldLayoutAttribute : Attribute, IFieldLayout
    {
        public FieldLayoutAttribute()
            : this(FieldLayout.PerUnit)
        {
        }

        public FieldLayoutAttribute(FieldLayout layout)
        {
            UnitNum = 1;
            Layout = layout;
        }

        [SimpleAttribute]
        public int UnitNum { get; set; }

        [SimpleAttribute]
        public FieldLayout Layout { get; private set; }

        public override string ToString()
        {
            if (Layout == FieldLayout.PerLine)
                return "[PerLine]";
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", Layout, UnitNum);
        }
    }
}
