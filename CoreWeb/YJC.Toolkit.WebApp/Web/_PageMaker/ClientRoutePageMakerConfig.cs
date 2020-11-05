using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-04-21", Author = "YJC",
        Description = "根据客户端的类型进行路由，采用匹配的PageMaker")]
    internal class ClientRoutePageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IDeviceService service = WebGlobalVariable.Context.RequestServices.GetService(
                typeof(IDeviceService)).Convert<IDeviceService>();

            IConfigCreator<IPageMaker> creator;
            switch (service.Device)
            {
                case DeviceType.PC:
                    creator = PC;
                    break;

                case DeviceType.Mobile:
                    creator = Mobile ?? PC;
                    break;

                case DeviceType.Pad:
                    creator = (Pad ?? Mobile) ?? PC;
                    break;

                default:
                    creator = PC;
                    break;
            }

            return creator.CreateObject(args);
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, NamingRule = NamingRule.Upper, Required = true)]
        public IConfigCreator<IPageMaker> PC { get; private set; }

        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IPageMaker> Mobile { get; private set; }

        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IPageMaker> Pad { get; private set; }
    }
}