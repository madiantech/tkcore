using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "将Upload的地址显示为图片的Display")]
    [ObjectContext]
    internal class UploadImgDisplayConfig : IDisplay, IConfigCreator<IDisplay>
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
                HtmlAttribute cssAttr = string.IsNullOrEmpty(CssClass) ? null
                    : new HtmlAttribute("class", CssClass);
                HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
                builder.Add("src", url);
                builder.Add(cssAttr);

                return string.Format(ObjectUtil.SysCulture, "<img {0} />", builder.CreateAttribute());
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

        [SimpleAttribute(LocalName = "Class")]
        public string CssClass { get; private set; }
    }
}