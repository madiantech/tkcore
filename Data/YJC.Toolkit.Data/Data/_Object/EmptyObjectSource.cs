using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class EmptyObjectSource : DynamicObjectSource
    {
        protected override void AddObject(IInputData input, dynamic bag)
        {
        }
    }
}
