namespace YJC.Toolkit.Sys
{
    public abstract class BaseXmlPlugInItem : IRegName, IAuthor, IXmlPlugInItem
    {
        protected BaseXmlPlugInItem()
        {
        }

        #region IRegName 成员

        [SimpleAttribute]
        public string RegName { get; protected set; }

        #endregion IRegName 成员

        #region IAuthor 成员

        [SimpleAttribute]
        public string Description { get; protected set; }

        [SimpleAttribute]
        public string Author { get; protected set; }

        [SimpleAttribute]
        public string CreateDate { get; protected set; }

        #endregion IAuthor 成员

        #region IXmlPlugInItem 成员

        public abstract string BaseClass { get; }

        #endregion IXmlPlugInItem 成员

        public override string ToString()
        {
            return string.IsNullOrEmpty(RegName) ? base.ToString() : string.Format(
                ObjectUtil.SysCulture, "注册名为{0}的配置单元", RegName);
        }
    }
}