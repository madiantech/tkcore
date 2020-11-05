using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IOperatorFieldProvider : IFieldValueProvider
    {
        ObjectOperatorCollection Operators { get; }
    }
}