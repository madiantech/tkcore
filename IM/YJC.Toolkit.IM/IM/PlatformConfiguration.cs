using System.Runtime.CompilerServices;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class PlatformConfiguration<T>
    {
        private IPlatformSettingCreator<T> fCreator;

        private void AssertCreator()
        {
            TkDebug.AssertNotNull(fCreator, "没有设置参数创建器，请先设置", null);
        }

        public T Create(string tenantId)
        {
            AssertCreator();

            return fCreator.Create(tenantId);
        }

        public AccessToken ReadToken(string tenantId, object customData)
        {
            AssertCreator();

            return fCreator.ReadToken(tenantId, customData);
        }

        public void SaveToken(string tenantId, object customData, AccessToken token)
        {
            AssertCreator();

            fCreator.SaveToken(tenantId, customData, token);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCreator(IPlatformSettingCreator<T> creator)
        {
            TkDebug.AssertArgumentNull(creator, "creator", null);
            TkDebug.Assert(fCreator == null,
                "已经设置过DingTalk的参数创建器，无法再次创建", null);

            fCreator = creator;
        }
    }
}