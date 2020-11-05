using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [RegType(Author = "YJC", CreateDate = "2015-09-09", Description = "文件上传的数据")]
    [TypeScheme(Author = "YJC", CreateDate = "2015-09-09", Description = "文件上传的元数据")]
    public class BaseUploadObject : IObjectUpload
    {
        #region IObjectUpload 成员

        [SimpleAttribute]
        [FieldInfo(Length = 255, IsKey = true)]
        [FieldControl(ControlType.Upload, Order = 100)]
        [FieldLayout(FieldLayout.PerLine)]
        [DisplayName("文件")]
        [FieldUpload(null, null, "WebPath", null, null)]
        public string FileName { get; protected set; }

        [SimpleAttribute]
        //[FieldControl(ControlType.Hidden, Order = 10)]
        //[FieldLayout]
        //[DisplayName("文件类型")]
        public string ContentType { get; protected set; }

        [SimpleAttribute]
        //[FieldControl(ControlType.Hidden, Order = 10)]
        //[FieldLayout]
        //[DisplayName("服务器路径")]
        public string ServerPath { get; protected set; }

        [SimpleAttribute]
        //[FieldControl(ControlType.Hidden, Order = 10)]
        //[FieldLayout]
        //[DisplayName("文件大小")]
        public int FileSize { get; protected set; }

        [SimpleAttribute]
        //[FieldControl(ControlType.Hidden, Order = 10)]
        //[FieldLayout]
        //[DisplayName("Web路径")]
        public string WebPath { get; protected set; }

        #endregion
    }
}
