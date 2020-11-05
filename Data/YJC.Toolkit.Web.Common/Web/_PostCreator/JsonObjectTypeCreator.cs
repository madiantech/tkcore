using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class JsonObjectTypeCreator : IPostObjectCreator
    {
        private Type fObjectType;

        public JsonObjectTypeCreator(Type objectType)
        {
            fObjectType = objectType;
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            object postObject = UseConstructor ? ObjectUtil.CreateObjectWithCtor(fObjectType)
                : ObjectUtil.CreateObject(fObjectType);
            postObject.ReadFromStream("Json", ModelName, stream, ObjectUtil.ReadSettings,
                QName.ToolkitNoNS);

            return postObject;
        }

        #endregion

        public bool UseConstructor { get; set; }

        public string ModelName { get; set; }
    }
}
