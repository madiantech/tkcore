using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RegRedirectorAttribute : BaseRedirectorAttrbiute
    {
        /// <summary>
        /// Initializes a new instance of the SourceRedirectorAttribute class.
        /// </summary>
        public RegRedirectorAttribute(Type objectType)
        {
            TkDebug.AssertArgumentNull(objectType, "objectType", null);

            ObjectType = objectType;
            TkDebug.Assert(ObjectUtil.IsSubType(typeof(IRedirector), objectType),
                string.Format(ObjectUtil.SysCulture,
                "对象{0}必须完成IRedirector接口，现在不是", objectType), null);
        }

        public Type ObjectType { get; private set; }

        public override IRedirector CreateRedirector(IPageData pageData)
        {
            IRedirector redirector = ObjectUtil.CreateObject(ObjectType).Convert<IRedirector>();
            TkDebug.AssertNotNull(redirector, string.Format(
                ObjectUtil.SysCulture, "无法根据类型{0}创建实例", ObjectType), this);
            return redirector;
        }
    }
}
