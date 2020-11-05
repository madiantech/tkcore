using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SingleDbDeleteSource : BaseSingleDbEditSource
    {
        protected SingleDbDeleteSource()
        {
        }

        public SingleDbDeleteSource(IEditDbConfig config)
            : base(config)
        {
        }

        private KeyData DoDeleteAction(IInputData input)
        {
            DefaultUpdateAction(input);

            DataRow row = MainRow;
            KeyData result = MainResolver.CreateKeyData(row);
            MainResolver.Delete(input);

            Commit(input);
            return result;
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return false;
        }

        public override OutputData DoAction(IInputData input)
        {
            PageStyle style = input.Style.Style;
            KeyData data = null;
            if (style == PageStyle.Delete)
            {
                try
                {
                    Prepare();
                    data = DoDeleteAction(input);
                }
                catch (WebPostException ex)
                {
                    return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                }
            }
            else
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "当前支持页面类型为Delete，当前类型是{0}", input.Style), this);

            return OutputData.CreateToolkitObject(data);
        }
    }
}