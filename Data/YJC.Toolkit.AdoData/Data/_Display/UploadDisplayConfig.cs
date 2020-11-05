using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "显示Upload的信息")]
    [ObjectContext]
    internal class UploadDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            if (DisplayUtil.IsNull(value))
                return string.Empty;

            if (field.Upload != null)
            {
                Tk5UploadConfig upload = field.AssertUpload();
                IUploadProcessor2 processor = upload.CreateUploadProcessor2();
                //IFieldValueProvider provider = new DataRowFieldValueProvider(row, row.Table.DataSet);
                string url = processor.Display(upload, rowValue);

                return string.Format(ObjectUtil.SysCulture, "<a href=\"{2}\" target=\"_blank\">{0}{1}</a>",
                    StringUtil.EscapeHtml(rowValue.GetValue(upload.FileNameField)),
                    BaseUploadProcessor.FormatSize(rowValue.GetValue<int>(upload.SizeField)),
                    StringUtil.EscapeHtmlAttribute(url));
            }
            return string.Empty;
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员
    }
}