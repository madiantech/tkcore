using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class HtmlCommonExtension
    {
        private const string ERROR_LABEL = " <label class=\"help-block\"></label>";
        private const int DEFAULT_UPLOAD_SIZE = 20 * 1024 * 1024;

        private static void AddNormalAttribute(Tk5FieldInfoEx field, HtmlAttributeBuilder builder,
            string name, bool needId, bool isHidden = false)
        {
            if (needId)
                builder.Add("id", name);
            builder.Add("name", name);
            string editClass = field.Edit == null ? null : field.Edit.Class;
            if (!isHidden)
            {
                builder.Add("class", HtmlCommonUtil.MergeClass("form-control", editClass));
                builder.Add("data-title", field.DisplayName);
            }
            else if (!string.IsNullOrEmpty(editClass))
                builder.Add("class", editClass);
            if (field.Edit != null)
                builder.AddRange(field.Edit.AttributeList);
        }

        private static void AddInputAttribute(Tk5FieldInfoEx field,
            HtmlAttributeBuilder builder, bool isSearchCtrl)
        {
            string hint = string.IsNullOrEmpty(field.Hint) || field.HintPosition != HintPosition.PlaceHolder
                ? field.DisplayName : field.Hint;
            if (!field.IsEmpty)
            {
                builder.Add((HtmlAttribute)"required");
                if (!isSearchCtrl)
                    hint += ", 必填";
            }
            if (field.PlaceHolder)
                builder.Add("placeholder", hint);
            if (field.Edit != null && field.Edit.ReadOnly)
                builder.Add((HtmlAttribute)"readonly");
        }

        private static void GetCheckValue(Tk5FieldInfoEx field, out string checkValue,
            out string uncheckValue)
        {
            if (field.Extension != null)
            {
                checkValue = field.Extension.CheckValue;
                uncheckValue = field.Extension.UnCheckValue;
            }
            else
            {
                checkValue = "1";
                uncheckValue = "0";
            }
        }

        private static string DateControl(Tk5FieldInfoEx field, string icon, string name,
            string value, string format, int size, bool needId, bool isSearchCtrl)
        {
            HtmlAttributeBuilder groupBuilder = new HtmlAttributeBuilder();
            groupBuilder.Add("data-control", field.InternalControl.Control);
            groupBuilder.Add("data-date-format", format);
            groupBuilder.Add("data-date", value);
            HtmlAttributeBuilder inputBuilder = new HtmlAttributeBuilder();
            inputBuilder.Add("type", "text");
            inputBuilder.Add("size", size);
            string deletestr;
            if (!field.ContainAttribute("unreadonly", true))
            {
                inputBuilder.Add((HtmlAttribute)"readonly");
                deletestr = Html.DateTimeDelete;
            }
            else
                deletestr = string.Empty;
            AddInputAttribute(field, inputBuilder, isSearchCtrl);
            AddNormalAttribute(field, inputBuilder, name, needId);
            inputBuilder.Add("value", value);

            return string.Format(ObjectUtil.SysCulture, Html.Datetime, groupBuilder.CreateAttribute(),
                inputBuilder.CreateAttribute(), icon, ERROR_LABEL, deletestr);
        }

        private static string InternalHidden(Tk5FieldInfoEx field, string value, bool needId)
        {
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.Add("type", "hidden");
            builder.Add("value", value);
            AddNormalAttribute(field, builder, field.NickName, needId, true);

            return string.Format(ObjectUtil.SysCulture, "<input {0}/>", builder.CreateAttribute());
        }

        private static string InternalInput(this Tk5FieldInfoEx field, string name, string value,
            bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            TkDebug.AssertNotNull(field.InternalControl, "field的InternalControl不能为空", field);
            builder.Add("type", field.InternalControl.Control);
            builder.Add("value", value);
            AddInputAttribute(field, builder, isSearchCtrl);
            AddNormalAttribute(field, builder, name, needId);

            if (field.HintPosition == HintPosition.PlaceHolder || string.IsNullOrEmpty(field.Hint))
                return string.Format(ObjectUtil.SysCulture, "<input {0}/>{1}",
                    builder.CreateAttribute(), ERROR_LABEL);
            else
            {
                string format;
                if (field.HintPosition == HintPosition.Front)
                    format = Html.InputHintBefore;
                else
                    format = Html.InputHintAfter;

                return string.Format(ObjectUtil.SysCulture, format, builder.CreateAttribute(),
                    ERROR_LABEL, field.Hint);
            }
        }

        //public static string EditCaption(this Tk5FieldInfoEx field, int col, string @class)
        //{
        //    TkDebug.AssertArgumentNull(field, "field", null);

        //    return BootcssUtil.EditCaption(field.NickName, field.DisplayName, col, @class);
        //}

        public static string TableDisplayClass(this TableDisplayType type)
        {
            if (type == TableDisplayType.Normal)
                return string.Empty;

            return "table-" + type.ToString().ToLower(ObjectUtil.SysCulture);
        }

        public static string GetKeyValue(this Dictionary<string, string> dictionary, string key)
        {
            if (dictionary == null)
                return string.Empty;
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            string result;
            if (dictionary.TryGetValue(key, out result))
                return result;
            return string.Empty;
        }

        public static string LayoutClass(this IFieldInfoEx field)
        {
            if (field == null)
                return string.Empty;

            IFieldLayout layout = field.Layout;
            if (layout == null)
                return string.Empty;
            switch (layout.Layout)
            {
                case FieldLayout.PerUnit:
                    if (layout.UnitNum == 1)
                        return string.Empty;
                    return "dc-" + layout.UnitNum;

                case FieldLayout.PerLine:
                    return "dc-all";
            }
            return string.Empty;
        }

        public static string AlginClass(this Alignment align)
        {
            if (align == Alignment.Left)
                return string.Empty;
            return "text-" + align.ToString().ToLower(ObjectUtil.SysCulture);
        }

        private static string GetSearchEndName(Tk5FieldInfoEx field)
        {
            string name = field.NickName + "END";
            return name;
        }

        private static string InternalTextArea(Tk5FieldInfoEx field, IFieldValueProvider row,
            HtmlAttribute addition, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            AddInputAttribute(field, builder, false);
            AddNormalAttribute(field, builder, field.NickName, needId);
            builder.Add(addition);

            return string.Format(ObjectUtil.SysCulture, "<textarea {0}>{1}</textarea>{2}",
                builder.CreateAttribute(), StringUtil.EscapeHtml(row[field.NickName].ToString()),
                ERROR_LABEL);
        }

        public static string Textarea(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return InternalTextArea(field, row, null, needId);
        }

        public static string RichText(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            HtmlAttribute ctrlAttr = new HtmlAttribute("data-control", field.InternalControl.Control);
            return InternalTextArea(field, row, ctrlAttr, needId);
        }

        public static string Input(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return Input(field, row, needId, false);
        }

        private static string Input(this Tk5FieldInfoEx field, IFieldValueProvider row,
        bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            object fieldValue = row[field.NickName]; // DisplayUtil.GetValue(field.NickName, row);
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);

            return InternalInput(field, field.NickName, value, needId, isSearchCtrl);
        }

        internal static string InputEnd(Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            string name = GetSearchEndName(field);
            object fieldValue = row[name]; // DisplayUtil.GetValue(name, row);
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);

            return InternalInput(field, name, value, true, true);
        }

        private static string InternalCombo(Tk5FieldInfoEx field, string name, IFieldValueProvider row,
            bool needId)
        {
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            AddNormalAttribute(field, builder, name, needId);

            TkDebug.AssertNotNull(field.Decoder, "Combo控件需要配置Decoder", field);
            DecoderAdditionInfo[] additions = null;
            if (field.Decoder.Additions != null)
            {
                additions = field.Decoder.Additions.ToArray();
                builder.Add("data-addition", additions.WriteJson());
            }
            //DataTable codeTable = model.Tables[field.Decoder.RegName];
            IEnumerable<IDecoderItem> codeTable = row.GetCodeTable(field.Decoder.RegName);
            StringBuilder options = new StringBuilder();
            if (field.IsEmpty)
            {
                string emptyTitle;
                if (field.Extension != null && field.Extension.EmptyTitle != null)
                    emptyTitle = field.Extension.EmptyTitle;
                else
                    emptyTitle = string.Empty;
                options.Append("<option value=\"\">").Append(emptyTitle).AppendLine("</option>");
            }
            string value = row[name].ToString();
            if (codeTable != null)
            {
                string addition = string.Empty;
                foreach (IDecoderItem codeRow in codeTable)
                {
                    string codeValue = codeRow.Value;
                    if (additions != null)
                    {
                        DecoderAdditionData data = new DecoderAdditionData();
                        data.AddData(codeRow, additions);
                        addition = " " + data.ToJson();
                    }

                    options.AppendFormat(ObjectUtil.SysCulture, "<option value=\"{0}\"{1}{3}>{2}</option>\r\n",
                        codeValue, codeValue == value ? " selected" : string.Empty, codeRow.Name, addition);
                }
            }

            return string.Format(ObjectUtil.SysCulture, "<select {0}>{1}</select>{2}",
                builder.CreateAttribute(), options, ERROR_LABEL);
        }

        public static string Combo(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            TkDebug.AssertArgumentNull(row, "row", null);

            return InternalCombo(field, field.NickName, row, needId);
        }

        internal static string ComboEnd(Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            string name = GetSearchEndName(field);

            return InternalCombo(field, name, row, true);
        }

        public static string RadioGroup(this Tk5FieldInfoEx field, IFieldValueProvider row,
            bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            TkDebug.AssertArgumentNull(row, "row", null);

            TkDebug.AssertNotNull(field.Decoder, "RadioGroup控件需要配置Decoder", field);
            var codeTable = row.GetCodeTable(field.Decoder.RegName);
            StringBuilder options = new StringBuilder();
            if (codeTable != null)
            {
                HtmlAttributeBuilder divBuilder = new HtmlAttributeBuilder();
                AddNormalAttribute(field, divBuilder, field.NickName, needId, true);
                divBuilder.Add("data-control", "RadioGroup");

                HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
                builder.Add("type", "radio");
                builder.Add("name", field.NickName);
                builder.Add("class", "radio-center");

                string value = row[field.NickName].ToString();
                options.AppendFormat(ObjectUtil.SysCulture, "<div {0}>\r\n", divBuilder.CreateAttribute());
                foreach (var codeRow in codeTable)
                {
                    string codeValue = codeRow.Value;
                    builder.Add("value", codeValue);
                    //options.AppendFormat(ObjectUtil.SysCulture,
                    //    "  <label class=\"checkbox-inline\"><input {0}{1}> {2}</label>\r\n",
                    //    builder.CreateAttribute(), codeValue == value ? " checked" : string.Empty,
                    //    codeRow.Name);
                    options.AppendFormat(ObjectUtil.SysCulture,
                        "  <div class=\"radio\"><input {0}{1} /><label class=\"radio-label\">{2}</label></div>\r\n",
                        builder.CreateAttribute(), codeValue == value ? " checked" : string.Empty,
                        codeRow.Name);
                }
                options.Append("</div>").AppendLine(ERROR_LABEL);
            }
            return options.ToString();
        }

        public static string CheckBoxList(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            TkDebug.AssertArgumentNull(row, "row", null);

            TkDebug.AssertNotNull(field.Decoder, "CheckBoxList控件需要配置Decoder", field);
            var codeTable = row.GetCodeTable(field.Decoder.RegName);
            StringBuilder options = new StringBuilder();
            if (codeTable != null)
            {
                HtmlAttributeBuilder divBuilder = new HtmlAttributeBuilder();
                AddNormalAttribute(field, divBuilder, field.NickName, needId, true);
                divBuilder.Add("data-control", "CheckBoxList");

                HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
                //builder.Add("type", "radio");
                //builder.Add("name", field.NickName);
                //builder.Add("class", "radio-center");

                string value = row[field.NickName].ToString();
                QuoteStringList quoteValue = QuoteStringList.FromString(value);
                options.AppendFormat(ObjectUtil.SysCulture, "<div {0}>\r\n", divBuilder.CreateAttribute());
                foreach (var codeRow in codeTable)
                {
                    builder.Clear();
                    string codeValue = codeRow.Value;
                    builder.Add("value", codeValue);
                    if (quoteValue.Contains(codeValue))
                        builder.Add((HtmlAttribute)"checked");
                    options.AppendFormat(ObjectUtil.SysCulture, Html.CheckBoxListItem,
                        builder.CreateAttribute(), codeRow.Name);
                }
                options.Append("</div>").AppendLine(ERROR_LABEL);
            }
            return options.ToString();
        }

        private static HtmlAttributeBuilder InternalCheckBox(Tk5FieldInfoEx field,
            IFieldValueProvider row, bool needId)
        {
            string checkValue;
            string uncheckValue;
            GetCheckValue(field, out checkValue, out uncheckValue);
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.Add("type", "checkbox");
            builder.Add("value", checkValue);
            builder.Add("data-uncheck-value", uncheckValue);
            if (row[field.NickName].ConvertToString() == checkValue)
                builder.Add((HtmlAttribute)"checked");
            AddNormalAttribute(field, builder, field.NickName, needId, true);
            return builder;
        }

        public static string CheckBox(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            HtmlAttributeBuilder builder = InternalCheckBox(field, row, needId);
            return string.Format(ObjectUtil.SysCulture, Html.CheckBox,
                builder.CreateAttribute(), field.DisplayName);
        }

        public static string Hidden(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return InternalHidden(field, row[field.NickName].ToString(), needId);
        }

        private static string InternalEasySearch(Tk5FieldInfoEx field, string nickName, IFieldValueProvider row,
            bool needId, bool isSearchCtrl)
        {
            HtmlAttributeBuilder hiddenBuilder = new HtmlAttributeBuilder();
            hiddenBuilder.Add("type", "hidden");
            hiddenBuilder.Add("value", row[nickName].ToString());
            string hiddenName = "hd" + nickName;
            if (needId)
                hiddenBuilder.Add("id", hiddenName);
            hiddenBuilder.Add("name", hiddenName);
            HtmlAttributeBuilder textBuilder = new HtmlAttributeBuilder();
            textBuilder.Add("type", "text");
            textBuilder.Add("data-regName", field.Decoder.RegName);
            AddInputAttribute(field, textBuilder, isSearchCtrl);
            AddNormalAttribute(field, textBuilder, nickName, needId);
            textBuilder.Add("value", row[DecoderConst.DECODER_TAG + nickName]);
            SimpleFieldDecoder decoder = field.Decoder as SimpleFieldDecoder;
            if (decoder != null)
            {
                if (decoder.RefFields != null)
                {
                    EasySearchRefConfig[] config = decoder.RefFields.ToArray();
                    textBuilder.Add("data-refFields", config.WriteJson(WriteSettings.Default));
                }
                if (decoder.Additions != null)
                {
                    textBuilder.Add("data-addition", decoder.Additions.WriteJson(WriteSettings.Default));
                }
            }

            string easyUrl = "c/source/C/EasySearch".AppVirutalPath();
            string dialogUrl = ("c/~source/C/EasySearchRedirect?RegName="
                + field.Decoder.RegName).AppVirutalPath();

            return string.Format(ObjectUtil.SysCulture, Html.EasySearch,
                hiddenBuilder.CreateAttribute(), textBuilder.CreateAttribute(), ERROR_LABEL,
                StringUtil.EscapeHtmlAttribute(easyUrl), StringUtil.EscapeHtmlAttribute(dialogUrl));
        }

        private static string InternalMultiEasySearch(Tk5FieldInfoEx field, string nickName,
            IFieldValueProvider row, bool needId)
        {
            HtmlAttributeBuilder textBuilder = new HtmlAttributeBuilder();
            textBuilder.Add("type", "text");
            textBuilder.Add("data-regName", field.Decoder.RegName);
            AddInputAttribute(field, textBuilder, false);
            AddNormalAttribute(field, textBuilder, nickName, needId);
            SimpleFieldDecoder decoder = field.Decoder as SimpleFieldDecoder;
            if (decoder != null && decoder.RefFields != null)
            {
                EasySearchRefConfig[] config = decoder.RefFields.ToArray();
                textBuilder.Add("data-refFields", config.WriteJson(WriteSettings.Default));
            }

            StringBuilder multiItems = new StringBuilder();
            string decodeValue = row[DecoderConst.DECODER_TAG + nickName].ToString();
            if (string.IsNullOrEmpty(decodeValue))
                multiItems.AppendFormat(Html.MultiEasySearchItem, "hidden", string.Empty, string.Empty);
            else
            {
                MultipleDecoderData data = MultipleDecoderData.ReadFromString(decodeValue);
                HtmlAttributeBuilder decodeBuilder = new HtmlAttributeBuilder();
                foreach (var item in data)
                {
                    decodeBuilder.Clear();
                    decodeBuilder.Add("data-id", item.Value);
                    decodeBuilder.Add("data-name", item.Name);

                    multiItems.AppendFormat(Html.MultiEasySearchItem, "multi-item",
                        item.Name, decodeBuilder.CreateAttribute());
                }
            }

            string easyUrl = "c/source/C/EasySearch".AppVirutalPath();
            string dialogUrl = ("c/~source/C/EasySearchRedirect?RegName="
                + field.Decoder.RegName).AppVirutalPath();

            return string.Format(ObjectUtil.SysCulture, Html.MultipleEasySearch,
                multiItems, textBuilder.CreateAttribute(), ERROR_LABEL,
                StringUtil.EscapeHtmlAttribute(easyUrl), StringUtil.EscapeHtmlAttribute(dialogUrl));
        }

        internal static string EasySearchEnd(Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            string name = GetSearchEndName(field);

            return InternalEasySearch(field, name, row, true, true);
        }

        private static string EasySearch(this Tk5FieldInfoEx field, IFieldValueProvider row,
            bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return InternalEasySearch(field, field.NickName, row, needId, isSearchCtrl);
        }

        public static string EasySearch(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return EasySearch(field, row, needId, false);
        }

        public static string MultipleEasySearch(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return InternalMultiEasySearch(field, field.NickName, row, needId);
        }

        internal static string DateEnd(Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            string name = GetSearchEndName(field);
            object fieldValue = row[field.NickName + "END"];
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);
            return DateControl(field, "calendar", name, value, "yyyy-mm-dd", 10, true, true);
        }

        private static string Date(this Tk5FieldInfoEx field, IFieldValueProvider row,
            bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            object fieldValue = row[field.NickName];
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);
            return DateControl(field, "calendar", field.NickName, value, "yyyy-mm-dd", 10, needId, isSearchCtrl);
        }

        public static string Date(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return Date(field, row, needId, false);
        }

        private static string DateTime(this Tk5FieldInfoEx field, IFieldValueProvider row,
            bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            object fieldValue = row[field.NickName];
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);
            return DateControl(field, "th", field.NickName, value, "yyyy-mm-dd hh:ii", 16, needId, isSearchCtrl);
        }

        public static string DateTime(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return DateTime(field, row, needId, false);
        }

        internal static string DateTimeEnd(Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            string name = GetSearchEndName(field);
            object fieldValue = row[name + "END"];
            string value = DisplayUtil.GetDisplayString(field.Edit.Display, fieldValue, field, row);
            return DateControl(field, "th", name, value, "yyyy-mm-dd hh:ii", 16, true, true);
        }

        private static string Time(this Tk5FieldInfoEx field, IFieldValueProvider row,
            bool needId, bool isSearchCtrl)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return DateControl(field, "time", field.NickName, string.Empty, "hh:ii", 5, needId, isSearchCtrl);
        }

        public static string Time(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            return Time(field, row, needId, false);
        }

        public static string Detail(this Tk5FieldInfoEx field, IFieldValueProvider row, bool showHint, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            AddNormalAttribute(field, builder, field.NickName, needId, true);
            object fieldValue = row[field.NickName];
            builder.Add("data-value", fieldValue.ToString());
            HtmlAttribute classAttr = builder["class"];
            if (classAttr == null)
                builder.Add("class", "detail-content");
            else
                classAttr.Value = HtmlCommonUtil.MergeClass(classAttr.Value, "detail-content");
            string displayValue = DisplayUtil.GetDisplayString(field.ListDetail.DetailDisplay,
                fieldValue, field, row);
            return string.Format(ObjectUtil.SysCulture, "<div {0}>{1}</div>",
                builder.CreateAttribute(), displayValue);
        }

        public static string DisplayUpload(this Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            Tk5UploadConfig upload = field.AssertUpload();
            IUploadProcessor2 processor = upload.CreateUploadProcessor2();
            return processor.Display(upload, row);
        }

        public static string DisplayValue(this Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            object fieldValue = row[field.NickName];
            return DisplayUtil.GetDisplayString(field.ListDetail.ListDisplay, fieldValue, field, row);
            //return DisplayValue(field, row, false);
        }

        public static string Switcher(this Tk5FieldInfoEx field, IFieldValueProvider row, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            HtmlAttributeBuilder builder = InternalCheckBox(field, row, needId);
            return string.Format(ObjectUtil.SysCulture, Html.Switcher, builder.CreateAttribute());
        }

        public static string Upload(this Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            var upload = field.AssertUpload();

            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.Add("data-control", "Upload");
            builder.Add("data-fileSize", upload.SizeField);
            builder.Add("data-serverPath", upload.ServerPathField);
            builder.Add("data-contentType", upload.MimeTypeField);
            AddNormalAttribute(field, builder, field.NickName, needId, true);

            int maxSize = upload.MaxSize;
            if (maxSize == 0)
                maxSize = BaseGlobalVariable.Current.DefaultValue
                    .GetSimpleDefaultValue("DefaultMaxUploadSize").Value<int>(DEFAULT_UPLOAD_SIZE);
            builder.Add("data-maxSize", maxSize);

            IUploadProcessor2 processor = upload.CreateUploadProcessor2();
            UploadInfo info = provider == null ? null : processor.CreateValue(upload, provider);
            if (info != null)
                builder.Add("data-value", info.ToJson());
            IUploadExtension extension = processor as IUploadExtension;
            if (extension != null)
            {
                string uploadUrl = extension.UploadUrl;
                if (!string.IsNullOrEmpty(uploadUrl))
                    builder.Add("data-url", uploadUrl);
            }

            return string.Format(ObjectUtil.SysCulture, Html.Upload, builder.CreateAttribute(),
                ERROR_LABEL);
        }

        internal const string SPAN_CTRL = "<span class=\"tk-control\">{0}</span>";

        public static String ControlHtml(this Tk5FieldInfoEx field,
            IFieldValueProvider provider, bool needId)
        {
            string ctrl = field.ControlName;
            BasePlugInFactory factroy = BaseGlobalVariable.Current
                .FactoryManager.GetCodeFactory(ControlHtmlPlugInFactory.REG_NAME);
            return GetCtrlHtml(field, provider, ctrl, factroy, needId);
        }

        private static string GetCtrlHtml(Tk5FieldInfoEx field, IFieldValueProvider provider,
            String ctrl, BasePlugInFactory factroy, bool needId)
        {
            if (factroy.Contains(ctrl))
            {
                IControlHtml html = factroy.CreateInstance<IControlHtml>(ctrl);
                return html.GetHtml(field, provider, needId);
            }
            else
                return string.Format(ObjectUtil.SysCulture, "<!--系统当前没有注册{0}的控件，请完善-->", ctrl);
        }

        public static string SearchControlHtml(this Tk5FieldInfoEx field, IFieldValueProvider provider)
        {
            ControlHtmlPlugInFactory factroy = BaseGlobalVariable.Current
                .FactoryManager.GetCodeFactory(ControlHtmlPlugInFactory.REG_NAME)
                .Convert<ControlHtmlPlugInFactory>();
            string ctrl = field.ControlName;
            string searchCtrl;
            bool isRange;
            if (field.ListDetail != null && field.ListDetail.Span)
            {
                searchCtrl = factroy.GetRangeControl(ctrl, field);
                isRange = true;
            }
            else
            {
                searchCtrl = factroy.GetSearchControl(ctrl, field);
                isRange = false;
            }
            bool changeEmpty = !field.IsEmpty;

            if (changeEmpty)
                field.IsEmpty = true;

            string res = GetCtrlHtml(field, provider, searchCtrl, factroy, true);
            if (!isRange)
                res = string.Format(ObjectUtil.SysCulture, SPAN_CTRL, res);

            if (changeEmpty)
                field.IsEmpty = false;
            return res;
        }

        public static string EditCaption(this Tk5FieldInfoEx field, int col, string @class)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return BootcssCommonUtil.EditCaption(field.NickName, field.DisplayName, col, @class);
        }
    }
}