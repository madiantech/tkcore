namespace YJC.Toolkit.Data
{
    public interface IFieldValueAccessor : IFieldValueProvider
    {
        object GetOriginValue(string nickName);

        void SetValue(string nickName, object value);
    }
}
