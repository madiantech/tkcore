using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class JsonFieldList
    {
        public JsonFieldList(string tableName, IEnumerable<Tk5FieldInfoEx> fields)
        {
            Table = tableName;
            var regList = new RegNameList<JsonField>();
            Fields = regList;
            if (fields == null)
                return;

            foreach (var field in fields)
            {
                ControlHtmlPlugInFactory factroy = BaseGlobalVariable.Current
                    .FactoryManager.GetCodeFactory(ControlHtmlPlugInFactory.REG_NAME)
                    .Convert<ControlHtmlPlugInFactory>();
                string ctrl = field.ControlName;
                var searchCtrl = factroy.GetSearchControl(ctrl, field);

                Fields.Add(new JsonField(field.NickName, searchCtrl));
                if (field.ListDetail != null && field.ListDetail.Span)
                    Fields.Add(new JsonField(field.NickName + "END", searchCtrl));
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