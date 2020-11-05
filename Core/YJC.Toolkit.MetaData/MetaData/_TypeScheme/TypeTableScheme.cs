using System;
using System.Collections.Generic;
using System.Reflection;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AlwaysCache]
    public class TypeTableScheme : ITableScheme, ITableSchemeEx
    {
        private readonly RegNameList<PropertyFieldInfo> fList;

        private const BindingFlags BIND_ATTRIBUTE =
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        internal TypeTableScheme(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            TableName = type.Name;
            DisplayNameAttribute dispAttr = Attribute.GetCustomAttribute(type,
                typeof(DisplayNameAttribute), false) as DisplayNameAttribute;
            if (dispAttr != null)
                TableDesc = dispAttr.DisplayName;
            else
                TableDesc = TableName;
            fList = new RegNameList<PropertyFieldInfo>();
            PropertyInfo[] props = type.GetProperties(BIND_ATTRIBUTE);
            if (props != null)
                foreach (var prop in props)
                {
                    PropertyFieldInfo info = PropertyFieldInfo.Create(prop);
                    if (info != null)
                        fList.Add(info);
                }

            NameField = MetaDataUtil.GetNameField(fList);
        }

        #region ITableScheme 成员

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string TableDesc { get; private set; }

        IEnumerable<IFieldInfo> ITableScheme.Fields
        {
            get
            {
                return fList;
            }
        }

        IEnumerable<IFieldInfo> ITableScheme.AllFields
        {
            get
            {
                return fList;
            }
        }

        #endregion ITableScheme 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fList[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region ITableSchemeEx 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, ObjectType = typeof(PropertyFieldInfo), LocalName = "Field")]
        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fList;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fList;
            }
        }

        public IFieldInfoEx NameField { get; }

        #endregion ITableSchemeEx 成员

        public override string ToString()
        {
            if (TableName == TableDesc)
                return string.Format(ObjectUtil.SysCulture, "[{0}]", TableName);
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", TableName, TableDesc);
        }

        public static TypeTableScheme Create(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            return CacheManager.GetItem("TypeTableScheme", type.FullName,
                type).Convert<TypeTableScheme>();
        }
    }
}