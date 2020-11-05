using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ParameterAttribute : Attribute
    {
        private TkDataType fDataType;

        public ParameterAttribute()
        {
            Direction = ParameterDirection.Input;
            LetterCase = CaseCategory.Uppercase;
            UseDefaultType = true;
        }

        /// <summary>
        /// Initializes a new instance of the ParameterAttribute class.
        /// </summary>
        public ParameterAttribute(ParameterDirection direction)
            : this()
        {
            Direction = direction;
        }

        public string Name { get; set; }

        public CaseCategory LetterCase { get; set; }

        public TkDataType DataType
        {
            get
            {
                return fDataType;
            }
            set
            {
                fDataType = value;
                UseDefaultType = false;
            }
        }

        public ParameterDirection Direction { get; private set; }

        public int Size { get; set; }

        internal bool UseDefaultType { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : string.Format(ObjectUtil.SysCulture,
                "名称为{0}的存储过程参数特性", Name);
        }
    }
}
