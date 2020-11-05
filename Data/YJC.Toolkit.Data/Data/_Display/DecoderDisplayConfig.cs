using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "EasySearch和CodeTable显示")]
    internal class DecoderDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string result = DisplayUtil.GetRowDecoderValue(rowValue, field.NickName).ConvertToString();
            return result;
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion
    }
}
