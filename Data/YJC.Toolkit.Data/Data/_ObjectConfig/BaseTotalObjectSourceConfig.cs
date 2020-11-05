using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal abstract class BaseTotalObjectSourceConfig : IConfigCreator<ISource>
    {
        protected BaseTotalObjectSourceConfig()
        {
        }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            CheckPageStyle(input.Style);

            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                    return CreateSource<IInsertObjectSource>(ObjectSource,
                        (source) => new InsertObjectSource(source));
                case PageStyle.Update:
                    return CreateSource<IUpdateObjectSource>(ObjectSource,
                        (source) => new UpdateObjectSource(source));
                case PageStyle.Delete:
                    return CreateSource<IDeleteObjectSource>(ObjectSource,
                        (source) => new DeleteObjectSource(source));
                case PageStyle.Detail:
                    var detailSource = CreateSource<IDetailObjectSource>(ObjectSource,
                        (source) => new DetailObjectSource(source));
                    if (DetailOperators != null)
                        detailSource.Convert<DetailObjectSource>().Operators = DetailOperators.CreateObject();
                    return detailSource;
                case PageStyle.List:
                    return CreateListSource(input);
                case PageStyle.Custom:
                    return CreateCustomSource(input);
            }
            return null;
        }

        #endregion

        [SimpleAttribute]
        public bool UseMetaData { get; protected set; }

        [SimpleAttribute]
        public string ObjectName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ObjectSourceConfig ObjectSource { get; protected set; }

        [DynamicElement(ObjectOperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IObjectOperatorsConfig> DetailOperators { get; protected set; }

        protected abstract ISource CreateListSource(IInputData input);

        protected virtual ISource CreateCustomSource(IInputData input)
        {
            return null;
        }

        protected virtual void CheckPageStyle(IPageStyle style)
        {
        }

        protected BaseObjectSource<T> CreateSource<T>(ObjectSourceConfig sourceConfig,
            Func<T, BaseObjectSource<T>> createFunc) where T : class
        {
            T objectSource = sourceConfig.CreateObjectSource().Convert<T>();
            BaseObjectSource<T> source = createFunc(objectSource);
            source.UseMetaData = UseMetaData;
            source.ObjectName = ObjectName;

            return source;
        }
    }
}
