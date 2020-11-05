using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EasySearchAttribute : BasePlugInAttribute
    {
        public EasySearchAttribute()
        {
            Suffix = "EasySearch";
        }

        internal EasySearchAttribute(IAuthor config)
            : this()
        {
            RegName = config.Convert<IRegName>().RegName;
            Author = config.Author;
            Description = config.Description;
            CreateDate = config.CreateDate;
        }

        public override string FactoryName
        {
            get
            {
                return EasySearchPlugInFactory.REG_NAME;
            }
        }
    }
}
