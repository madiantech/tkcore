using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class JsonFieldList
    {
        public JsonFieldList(string tableName, IEnumerable<Tk5FieldInfoEx> fields)
            : this(tableName, fields, false)
        {
        }

        public JsonFieldList(string tableName, IEnumerable<Tk5FieldInfoEx> fields, bool hasEditKey)
        {
            Table = tableName;
            var regList = new RegNameList<JsonField>();
            Fields = regList;
            foreach (var field in fields)
            {
                if (hasEditKey && field.IsKey)
                    Fields.Add(new JsonField("OLD_" + field.NickName, "Hidden"));

                Fields.Add(new JsonField(field));
                if (field.ListDetail != null && field.ListDetail.Span)
                    Fields.Add(new JsonField(field.NickName + "END", field));
            }

            // 处理Upload
            var uploadFields = from item in fields
                               where item.InternalControl.SrcControl == ControlType.Upload && item.Upload != null
                               select item;
            foreach (var field in uploadFields)
            {
                var upload = field.Upload;
                RemoveItem(regList, upload.SizeField);
                RemoveItem(regList, upload.ContentField);
                RemoveItem(regList, upload.MimeTypeField);
                RemoveItem(regList, upload.ServerPathField);
            }
        }

        [SimpleElement(Order = 10)]
        public string Table { get; private set; }

        [SimpleElement(Order = 20)]
        public SearchControlMethod SearchMethod { get; set; }

        [SimpleElement(Order = 30)]
        public JsonObjectType JsonType { get; set; }

        [ObjectElement(IsMultiple = true, Order = 40)]
        public IList<JsonField> Fields { get; private set; }

        private static void RemoveItem(RegNameList<JsonField> regList, string name)
        {
            var item = regList[name];
            if (item != null)
                regList.Remove(item);
        }

        public string ToJsonString()
        {
            WriteSettings settings = new WriteSettings { Encoding = Encoding.Default };
            return this.WriteJson(settings);
        }
    }
}
