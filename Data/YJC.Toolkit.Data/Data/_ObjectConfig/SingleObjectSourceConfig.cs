using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-12-02", Author = "YJC",
        Description = "将List，Detail，Edit的单个对象操作的数据源整合在一起的数据源，完成对象的增删改工作")]
    internal class SingleObjectSourceConfig : BaseTotalObjectSourceConfig
    {
        #region IConfigCreator<ISource> 成员

        //public ISource CreateObject(params object[] args)
        //{
        //    IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

        //    if ((input.Style.Style & DisablePage) == input.Style.Style)
        //        throw new ErrorOperationException("该页面被禁止，无法访问", this);

        //    switch (input.Style.Style)
        //    {
        //        case PageStyle.Insert:
        //            return CreateSource<IInsertObjectSource>(ObjectSource, 
        //                (source) => new InsertObjectSource(source));
        //        case PageStyle.Update:
        //            return CreateSource<IUpdateObjectSource>(ObjectSource, 
        //                (source) => new UpdateObjectSource(source));
        //        case PageStyle.Delete:
        //            return CreateSource<IDeleteObjectSource>(ObjectSource, 
        //                (source) => new DeleteObjectSource(source));
        //        case PageStyle.Detail:
        //            var detailSource = CreateSource<IDetailObjectSource>(ObjectSource, 
        //                (source) => new DetailObjectSource(source));
        //            if (DetailOperators != null)
        //                detailSource.Convert<DetailObjectSource>().Operators = DetailOperators.CreateObject();
        //            return detailSource;
        //        case PageStyle.List:
        //            var listSource = CreateSource<IListObjectSource>(ListObjectSource ?? ObjectSource, 
        //                (source) => new ListObjectSource(source));
        //            var list = listSource.Convert<ListObjectSource>();
        //            list.PageSize = PageSize;
        //            if (Operators != null)
        //                list.Operators = Operators.CreateObject();
        //            return list;
        //    }
        //    return null;
        //}

        #endregion

        [SimpleAttribute]
        public PageStyle DisablePage { get; private set; }

        [SimpleAttribute]
        public int PageSize { get; private set; }

        //[SimpleAttribute]
        //public bool UseMetaData { get; private set; }

        //[SimpleAttribute]
        //public string ObjectName { get; private set; }

        //[ObjectElement(NamespaceType.Toolkit)]
        //public ObjectSourceConfig ObjectSource { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ObjectSourceConfig ListObjectSource { get; private set; }

        [DynamicElement(ObjectOperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IObjectOperatorsConfig> Operators { get; private set; }

        //[DynamicElement(ObjectOperatorsConfigFactory.REG_NAME)]
        //[TagElement(NamespaceType.Toolkit)]
        //public IConfigCreator<IObjectOperatorsConfig> DetailOperators { get; private set; }

        //private BaseObjectSource<T> CreateSource<T>(ObjectSourceConfig sourceConfig, 
        //    Func<T, BaseObjectSource<T>> createFunc) where T : class
        //{
        //    T objectSource = sourceConfig.CreateObjectSource().Convert<T>();
        //    BaseObjectSource<T> source = createFunc(objectSource);
        //    source.UseMetaData = UseMetaData;
        //    source.ObjectName = ObjectName;

        //    return source;
        //}

        protected override void CheckPageStyle(IPageStyle style)
        {
            if ((style.Style & DisablePage) == style.Style)
                throw new ErrorOperationException("该页面被禁止，无法访问", this);
        }

        protected override ISource CreateListSource(IInputData input)
        {
            var listSource = CreateSource<IListObjectSource>(ListObjectSource ?? ObjectSource,
                (source) => new ListObjectSource(source));
            var list = listSource.Convert<ListObjectSource>();
            list.PageSize = PageSize;
            if (Operators != null)
                list.Operators = Operators.CreateObject();
            return list;
        }
    }
}
