using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class Tk5TableScheme : ITableSchemeEx, IRegName, IFileDependency
    {
        private readonly RegNameList<Tk5FieldInfoEx> fList;

        private Tk5TableScheme()
        {
            fList = new RegNameList<Tk5FieldInfoEx>();
        }

        public Tk5TableScheme(Tk5TableScheme scheme, IEnumerable<Tk5FieldInfoEx> fields)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgumentNull(fields, "fields", null);

            SetFileDependency(scheme);
            fList = scheme.fList;
            TableName = scheme.TableName;
            TableDesc = scheme.TableDesc;
            NameField = scheme.NameField;
            foreach (var item in fields)
                fList.Add(item);

            ProcessRefField();
        }

        public Tk5TableScheme(ITableSchemeEx scheme, IInputData input, ISingleMetaData config,
            Func<ITableSchemeEx, IFieldInfoEx, IInputData, ISingleMetaData, Tk5FieldInfoEx> createFunc)
            : this()
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgumentNull(input, "input", null);
            TkDebug.AssertArgumentNull(config, "config", null);
            TkDebug.AssertArgumentNull(createFunc, "createFunc", null);

            SetFileDependency(scheme);
            TableName = scheme.TableName;
            TableDesc = scheme.TableDesc;
            NameField = scheme.NameField;

            PageStyle pageStyle = input.Style.Style;
            var list = from item in scheme.Fields
                       where (item.Control.DefaultShow & pageStyle) == pageStyle
                       select item;

            foreach (var item in list)
            {
                Tk5FieldInfoEx fieldInfo = createFunc(scheme, item, input, config);
                fList.Add(fieldInfo);
            }

            ProcessRefField();
        }

        internal Tk5TableScheme(ITableSchemeEx scheme, IInputData input, BaseSingleMetaDataConfig config,
            Func<ITableSchemeEx, IFieldInfoEx, IInputData, BaseSingleMetaDataConfig, Tk5FieldInfoEx> createFunc)
            : this()
        {
            SetFileDependency(scheme);
            if (!string.IsNullOrEmpty(config.TableName))
                TableName = config.TableName;
            else
                TableName = scheme.TableName;
            if (config.TableDesc != null)
                TableDesc = config.TableDesc.ToString();
            else
                TableDesc = scheme.TableDesc;
            NameField = scheme.NameField;

            PageStyle pageStyle = input.Style.Style;
            var list = from item in scheme.Fields
                       where IsShow(item, pageStyle, config.OverrideFields)
                       select item;

            foreach (var item in list)
            {
                Tk5FieldInfoEx fieldInfo = createFunc(scheme, item, input, config);
                fList.Add(fieldInfo);
            }

            // 删除主Schema中的字段
            DelFields(config.DelFields);
            // 重载主Schema中的字段
            OverrideFields(input, config.OverrideFields);
            // 添加虚拟字段到主Schema中
            AddFields(input, config.AddFields);

            ProcessRefField();
        }

        #region ITableSchemeEx 成员

        public string TableName { get; private set; }

        public IFieldInfoEx NameField { get; }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fList;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fList;
            }
        }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fList[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return TableName;
            }
        }

        #endregion IRegName 成员

        public string TableDesc { get; private set; }

        public RegNameList<Tk5FieldInfoEx> List
        {
            get
            {
                return fList;
            }
        }

        public IEnumerable<string> Files { get; private set; }

        private static bool IsShow(IFieldInfoEx field, PageStyle pageStyle,
            RegNameList<OverrideFieldConfig> overrideFields)
        {
            bool result = (field.Control.DefaultShow & pageStyle) == pageStyle;
            if (!result)
            {
                if (overrideFields != null)
                    result = overrideFields.ConstainsKey(field.NickName);
            }
            return result;
        }

        private void SetFileDependency(ITableSchemeEx scheme)
        {
            Files = FileUtil.GetFileDependecy(scheme);
        }

        private void OverrideFields(IInputData input, RegNameList<OverrideFieldConfig> overrideFields)
        {
            if (overrideFields != null)
            {
                foreach (var field in overrideFields)
                {
                    Tk5FieldInfoEx item = fList[field.NickName];
                    if (item != null)
                    {
                        if (field.IsEmpty.HasValue)
                            item.IsEmpty = field.IsEmpty.Value;
                        if (field.PlaceHolder.HasValue)
                            item.PlaceHolder = field.PlaceHolder.Value;
                        if (field.Length.HasValue)
                            item.Length = field.Length.Value;

                        if (field.Order.HasValue)
                            item.InternalControl.Order = field.Order.Value;
                        if (field.Control.HasValue)
                        {
                            item.InternalControl.SrcControl = field.Control.Value;
                            item.SetControl(input.Style);
                        }
                        if (field.DisplayName != null)
                            item.DisplayName = field.DisplayName.ToString();
                        if (field.Hint != null)
                        {
                            item.Hint = field.Hint.ToString();
                            item.HintPosition = field.Hint.Position;
                        }
                        if (field.CodeTable != null)
                            item.Decoder = new SimpleFieldDecoder(field.CodeTable, DecoderType.CodeTable);
                        else if (field.EasySearch != null)
                            item.Decoder = new SimpleFieldDecoder(field.EasySearch, DecoderType.EasySearch);

                        if (field.Layout != null)
                            ((SimpleFieldLayout)item.Layout).Override(field.Layout);
                        switch (input.Style.Style)
                        {
                            case PageStyle.Insert:
                            case PageStyle.Update:
                                OverrideEditConfig ovEdit = field.Edit;
                                if (ovEdit != null)
                                {
                                    if (item.Edit == null)
                                        item.Edit = new Tk5EditConfig();
                                    item.Edit.Override(ovEdit);
                                }
                                break;

                            case PageStyle.Detail:
                            case PageStyle.List:
                                var ovLD = field.ListDetail;
                                if (ovLD != null)
                                {
                                    if (item.ListDetail == null)
                                        item.ListDetail = new Tk5ListDetailConfig();
                                    item.ListDetail.Override(ovLD);
                                }
                                break;
                        }
                        var ovExt = field.Extension;
                        if (ovExt != null)
                        {
                            if (item.Extension == null)
                                item.Extension = new Tk5ExtensionConfig();
                            item.Extension.Override(item, ovExt);
                        }
                    }
                }
            }
        }

        private void DelFields(List<BaseFieldConfig> delFields)
        {
            if (delFields != null)
            {
                foreach (var field in delFields)
                {
                    var item = fList[field.NickName];
                    if (item != null)
                        fList.Remove(item);
                }
            }
        }

        private void AddFields(IInputData input, List<AddFieldConfig> addFields)
        {
            if (addFields != null)
            {
                foreach (var field in addFields)
                {
                    Tk5FieldInfoEx fieldInfo = new Tk5FieldInfoEx(field, input.Style);
                    fieldInfo.SetControl(input.Style);
                    fList.Add(fieldInfo);
                }
            }
        }

        private void ProcessRefField()
        {
            foreach (var field in fList)
            {
                field.ProcessRefField(fList);
            }
        }
    }
}