namespace YJC.Toolkit.Sys
{
    internal class ToolkitXmlObject<T> : ICustomReader
    {
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
            return new CustomPropertyInfo(typeof(T),
                new ObjectElementAttribute { UseConstructor = UseConstructor });
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            Data = (T)value;
        }

        #endregion ICustomReader 成员

        public T Data { get; private set; }

        public bool UseConstructor { get; set; }
    }
}