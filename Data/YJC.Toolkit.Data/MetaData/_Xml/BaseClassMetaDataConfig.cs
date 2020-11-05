using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class BaseClassMetaDataConfig : BaseSingleMetaDataConfig
    {
        [SimpleAttribute(Required = true)]
        public string ClassRegName { get; protected set; }

        public override ITableSchemeEx CreateSourceScheme(IInputData input)
        {
            TypeTableScheme schema = MetaDataUtil.CreateTypeTableScheme(ClassRegName);
            TkDebug.AssertNotNull(schema, string.Format(ObjectUtil.SysCulture,
                "没有找到标记了TypeTableSchemeAttribute的注册类{0}", ClassRegName), this);

            return schema;
        }

        public override Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input)
        {
            return new Tk5TableScheme(scheme, input, this, SchemeUtil.CreatePropertyField);
        }
    }
}
