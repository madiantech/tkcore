namespace YJC.Toolkit.IM
{
    public interface IPlatformSettingCreator<T>
    {
        T Create(string tenantId);

        AccessToken ReadToken(string tenantId, object customData);

        void SaveToken(string tenantId, object customData, AccessToken token);
    }
}
