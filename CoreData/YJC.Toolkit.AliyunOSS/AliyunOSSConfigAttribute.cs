using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AliyunOSSConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return AliyunOSSConfigFactory.REG_NAME;
            }
        }
    }
}
