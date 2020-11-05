using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CustomJsonObjectCreatorAttribute : BasePostObjectCreatorAttribute
    {
        public CustomJsonObjectCreatorAttribute(Type type, string localName)
        {
            TkDebug.AssertArgumentNull(type, "type", null);
            TkDebug.AssertArgumentNullOrEmpty(localName, "localName", null);

            Type = type;
            LocalName = localName;
        }

        public Type Type { get; private set; }

        public string LocalName { get; private set; }

        public bool UseConstructor { get; set; }

        public string ModelName { get; set; }

        public override IPostObjectCreator CreatePostObjectCreator(IPageData pageData)
        {
            return new CustomJsonObjectCreator(Type, LocalName)
            {
                UseConstructor = UseConstructor,
                ModelName = ModelName
            };
        }
    }
}
