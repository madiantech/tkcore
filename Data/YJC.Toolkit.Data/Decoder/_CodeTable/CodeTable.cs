using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class CodeTable : IDecoder
    {
        private BasePlugInAttribute fAttribute;

        protected CodeTable()
        {
        }

        #region IDecoder 成员

        public abstract IDecoderItem Decode(string code, params object[] args);

        public abstract void Fill(params object[] args);

        #endregion

        public virtual BasePlugInAttribute Attribute
        {
            get
            {
                if (fAttribute == null)
                    fAttribute = System.Attribute.GetCustomAttribute(GetType(),
                        typeof(CodeTableAttribute)) as BasePlugInAttribute;
                return fAttribute;
            }
        }

        public string RegName
        {
            get
            {
                BasePlugInAttribute attr = Attribute;
                TkDebug.AssertNotNull(attr, "CodeTable对象没有附着对应的Attribute", this);
                return attr.GetRegName(GetType());
            }
        }

        public override string ToString()
        {
            BasePlugInAttribute attr = Attribute;
            return attr == null ? base.ToString() : string.Format(ObjectUtil.SysCulture,
                "注册名为{0}的CodeTable", attr.GetRegName(GetType()));
        }
    }
}
