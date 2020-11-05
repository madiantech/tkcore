namespace YJC.Toolkit.Sys
{
    public abstract class BaseTypeConverter<T> : ITkTypeConverter, IConvertStatus
    {
        public string DefaultValue
        {
            get
            {
                object result = default(T);
                return result.ConvertToString();
            }
        }

        public bool IsSuccess { get; private set; }

        protected abstract object InternalConvertFromString(string text, ReadSettings settings);

        public virtual object ConvertFromString(string text, ReadSettings settings)
        {
            IsSuccess = true;
            try
            {
                return InternalConvertFromString(text, settings);
            }
            catch
            {
                IsSuccess = false;
                return default(T);
            }
        }

        public virtual string ConvertToString(object value, WriteSettings settings)
        {
            return value.ConvertToString();
        }
    }
}
