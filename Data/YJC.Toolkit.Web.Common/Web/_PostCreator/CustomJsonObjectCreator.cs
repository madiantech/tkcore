using System;
using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class CustomJsonObjectCreator : IPostObjectCreator
    {
        private readonly Type fType;
        private readonly string fLocalName;

        public CustomJsonObjectCreator(Type type, string localName)
        {
            fLocalName = localName;
            fType = type;
        }

        public CustomJsonObjectCreator(string regName, string localName)
            : this(ObjectUtil.GetRegType(regName), localName)
        {
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            JsonPostData postObject = new JsonPostData(fType, fLocalName)
            {
                UseConstructor = UseConstructor
            };
            postObject.ReadFromStream("Json", ModelName, stream, ObjectUtil.ReadSettings, QName.ToolkitNoNS);

            return postObject.Data;
        }

        #endregion

        public bool UseConstructor { get; set; }

        public string ModelName { get; set; }
    }
}
