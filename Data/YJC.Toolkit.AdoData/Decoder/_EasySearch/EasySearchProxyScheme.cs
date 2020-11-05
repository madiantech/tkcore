using System.Collections.Generic;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    internal class EasySearchProxyScheme : ITableScheme, ICacheDependencyCreator
    {
        private readonly ITableScheme fScheme;
        private readonly IDisplayObject fDisplayObject;
        private readonly KeyFieldItem fKeyField;
        private readonly FieldItem fNameField;
        private readonly ICacheDependency fCacheDependency;

        public EasySearchProxyScheme(ITableScheme scheme, IDisplayObject displayObject)
        {
            var creator = scheme as ICacheDependencyCreator;
            if (creator != null)
                fCacheDependency = creator.CreateCacheDependency();
            else
                fCacheDependency = AlwaysDependency.Dependency;

            fScheme = scheme;
            fDisplayObject = displayObject;
            IFieldInfo info = displayObject.Id;
            fKeyField = new KeyFieldItem(info.FieldName, DecoderConst.CODE_NICK_NAME, info.DataType);
            info = displayObject.Name;
            fNameField = new FieldItem(info.FieldName, DecoderConst.NAME_NICK_NAME, info.DataType);
        }

        #region ITableScheme 成员

        public string TableName
        {
            get
            {
                return fScheme.TableName;
            }
        }

        public IEnumerable<IFieldInfo> Fields
        {
            get
            {
                foreach (IFieldInfo item in fScheme.Fields)
                {
                    if (item.NickName == fDisplayObject.Id.NickName)
                        yield return KeyField;
                    else if (item.NickName == fDisplayObject.Name.NickName)
                        yield return fNameField;
                    else if (!(item.NickName == DecoderConst.CODE_NICK_NAME
                        || item.NickName == DecoderConst.NAME_NICK_NAME))
                        if (!item.IsKey)
                            yield return item;
                }
            }
        }

        public IEnumerable<IFieldInfo> AllFields
        {
            get
            {
                return Fields;
            }
        }

        #endregion ITableScheme 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                switch (nickName)
                {
                    case DecoderConst.CODE_NICK_NAME:
                        return fKeyField;

                    case DecoderConst.NAME_NICK_NAME:
                        return fNameField;

                    default:
                        return fScheme[nickName];
                }
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region ICacheDependencyCreator 成员

        public virtual ICacheDependency CreateCacheDependency()
        {
            return fCacheDependency;
        }

        #endregion ICacheDependencyCreator 成员

        public KeyFieldItem KeyField
        {
            get
            {
                return fKeyField;
            }
        }
    }
}