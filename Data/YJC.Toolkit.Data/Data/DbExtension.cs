
namespace YJC.Toolkit.Data
{
    public static class DbExtension
    {
        public static bool Contains(this UpdateKind container, UpdateKind kind)
        {
            return (container & kind) == kind;
        }
    }
}
