using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    internal class CustomObject : BaseResult, ICustomReader
    {
        private readonly bool fIsSimple;
        private readonly bool fUseConstructor;
        private readonly ITkTypeConverter fConverter;
        private readonly Type fType;
        private readonly bool fIsMultiple;
        private readonly Type fCollectionType;
        private readonly string fLocalName;

        private CustomObject(string localName, Type type)
        {
            fLocalName = localName;
            fType = type;
        }

        public CustomObject(string localName, Type simpleType, ITkTypeConverter converter)
            : this(localName, simpleType)
        {
            fIsSimple = true;
            fConverter = converter;
        }

        public CustomObject(string localName, Type simpleType, ITkTypeConverter converter,
            Type collectionType)
            : this(localName, simpleType, converter)
        {
            fIsMultiple = true;
            fCollectionType = collectionType;
        }

        public CustomObject(string localName, Type objectType, bool useConstructor)
            : this(localName, objectType)
        {
            fIsSimple = false;
            fUseConstructor = useConstructor;
        }

        public CustomObject(string localName, Type objectType, bool useConstructor,
            Type collectionType)
            : this(localName, objectType, useConstructor)
        {
            fIsMultiple = true;
            fCollectionType = collectionType;
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
            if (localName != fLocalName)
                return null;

            if (fIsSimple)
            {
                SimpleElementAttribute attr = new SimpleElementAttribute
                {
                    IsMultiple = fIsMultiple,
                    CollectionType = fCollectionType
                };
                return new CustomPropertyInfo(fType, attr, fConverter);
            }
            else
            {
                ObjectElementAttribute attr = new ObjectElementAttribute
                {
                    UseConstructor = fUseConstructor,
                    IsMultiple = fIsMultiple,
                    CollectionType = fCollectionType
                };
                return new CustomPropertyInfo(fType, attr);
            }
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            Data = value;
        }

        #endregion ICustomReader 成员

        public object Data { get; private set; }
    }
}