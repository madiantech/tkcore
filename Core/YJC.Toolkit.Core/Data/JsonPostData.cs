using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class JsonPostData : ICustomReader
    {
        private readonly Type fType;
        private readonly string fLocalName;

        public JsonPostData(Type type, string localName)
        {
            TkDebug.AssertArgumentNull(type, "type", null);
            TkDebug.AssertArgumentNullOrEmpty(localName, "localName", null);

            fLocalName = localName;
            fType = type;
        }

        #region ICustomReader 成员

        public bool SupportVersion
        {
            get
            {
                return false;
            }
        }

        public CustomPropertyInfo CanRead(string localName, string version)
        {
            if (localName == fLocalName)
                return new CustomPropertyInfo(fType,
                    new ObjectElementAttribute { UseConstructor = UseConstructor });

            return null;
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            if (localName == fLocalName)
                Data = value;
        }

        #endregion ICustomReader 成员

        public object Data { get; private set; }

        public bool UseConstructor { get; set; }
    }

    public class JsonPostData<T> : ICustomReader
    {
        private readonly string fLocalName;

        public JsonPostData(string localName)
        {
            TkDebug.AssertArgumentNullOrEmpty(localName, "localName", null);

            fLocalName = localName;
        }

        #region ICustomReader 成员

        public bool SupportVersion
        {
            get
            {
                return false;
            }
        }

        public CustomPropertyInfo CanRead(string localName, string version)
        {
            if (localName == fLocalName)
                return new CustomPropertyInfo(typeof(T),
                    new ObjectElementAttribute { UseConstructor = UseConstructor });

            return null;
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            if (localName == fLocalName)
                Data = (T)value;
        }

        #endregion ICustomReader 成员

        public T Data { get; private set; }

        public bool UseConstructor { get; set; }
    }
}