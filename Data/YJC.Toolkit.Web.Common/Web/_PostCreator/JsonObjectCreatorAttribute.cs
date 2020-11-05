using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonObjectCreatorAttribute : BasePostObjectCreatorAttribute
    {
        public JsonObjectCreatorAttribute(string regClassName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regClassName, "regClassName", null);

            RegClassName = regClassName;
        }

        public JsonObjectCreatorAttribute(Type regType)
        {
            TkDebug.AssertArgumentNull(regType, "regType", null);

            RegType = regType;
        }

        public override IPostObjectCreator CreatePostObjectCreator(IPageData pageData)
        {
            if (RegType == null)
                return new JsonObjectCreator(RegClassName)
                {
                    ModelName = ModelName
                };
            else
                return new JsonObjectTypeCreator(RegType)
                {
                    ModelName = ModelName
                };
        }

        public string RegClassName { get; private set; }

        public Type RegType { get; private set; }

        public string ModelName { get; set; }
    }
}
