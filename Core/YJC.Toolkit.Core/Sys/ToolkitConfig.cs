namespace YJC.Toolkit.Sys
{
    public class ToolkitConfig : IReadObjectCallBack, IDataFile
    {
        private string fFullPath;

        #region IReadObjectCallBack 成员

        void IReadObjectCallBack.OnReadObject()
        {
            if (!string.IsNullOrEmpty(ExpectedVersion))
                if (ExpectedVersion != Version)
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                        "Version的版本不对，期望的版本是\"{0}\"，而实际XML中是\"{1}\"",
                        ExpectedVersion, Version), this);
            OnReadObject();
        }

        #endregion IReadObjectCallBack 成员

        #region IDataFile 成员

        [SimpleAttribute]
        public string FullPath
        {
            get
            {
                return fFullPath;
            }
            set
            {
                //TkDebug.AssertArgumentNullOrEmpty(value, "value", this);

                if (fFullPath != value)
                {
                    fFullPath = value;
                    OnSetFullPath(value);
                }
            }
        }

        #endregion IDataFile 成员

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Version { get; protected set; }

        public string ExpectedVersion { get; set; }

        protected virtual void OnReadObject()
        {
        }

        protected virtual void OnSetFullPath(string path)
        {
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(FullPath) ? base.ToString() :
                FullPath + "配置";
        }
    }
}