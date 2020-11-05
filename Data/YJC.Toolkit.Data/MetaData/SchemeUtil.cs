using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal static class SchemeUtil
    {
        public static Tk5FieldInfoEx CreatePropertyField(ITableSchemeEx scheme,
            IFieldInfoEx field, IInputData input, BaseSingleMetaDataConfig config)
        {
            return new Tk5FieldInfoEx(field.Convert<PropertyFieldInfo>(), input.Style);
        }

        public static Tk5FieldInfoEx CreateDataXmlField(ITableSchemeEx scheme,
            IFieldInfoEx field, IInputData input, BaseSingleMetaDataConfig config)
        {
            Tk5FieldInfoEx fieldInfo = new Tk5FieldInfoEx(field, input.Style);
            //Tk5DataXml dataXml = scheme.Convert<Tk5DataXml>();
            IDisplayObject display = scheme as IDisplayObject;

            if (display != null && input.Style.Style == PageStyle.List)
            {
                bool isNameField = display.SupportDisplay && field.FieldName == display.Name.FieldName;
                if (!config.DisableAutoDetailLink)
                {
                    if (!isNameField && fieldInfo.ListDetail != null)
                    {
                        IConfigCreator<IDisplay> iConfigCreator = fieldInfo.ListDetail.ListDisplay;
                        IHrefDisplay hrefDisplay = iConfigCreator as IHrefDisplay;
                        if (hrefDisplay != null)
                        {
                            if (string.IsNullOrEmpty(hrefDisplay.Content))
                                hrefDisplay.Content = CreateContent(input, display);
                        }
                    }
                    if (isNameField)
                    {
                        if (fieldInfo.ListDetail == null)
                            fieldInfo.ListDetail = new Tk5ListDetailConfig();
                        string content = CreateContent(input, display);
                        if (fieldInfo.ListDetail.Link == null)
                        {
                            fieldInfo.ListDetail.Link = new Tk5LinkConfig(content);
                        }
                        IHrefDisplay hrefDisplay = fieldInfo.ListDetail.ListDisplay as IHrefDisplay;
                        if (hrefDisplay == null)
                        {
                            string displayXml = string.Format(ObjectUtil.SysCulture,
                                "<tk:ListRefDisplay><tk:NormalDisplay/></tk:ListRefDisplay>", content);
                            var listRefDisplay = Tk5FieldInfoEx.GetDisplay(displayXml);
                            if (fieldInfo.ListDetail.ListDisplay != null)
                            {
                                var displayContainer = listRefDisplay as IDisplayContainer;
                                if (displayContainer != null)
                                    displayContainer.SetInternalDisplay(fieldInfo.ListDetail.ListDisplay);
                            }
                            fieldInfo.ListDetail.ListDisplay = listRefDisplay;
                            hrefDisplay = listRefDisplay.Convert<IHrefDisplay>();
                        }
                        if (string.IsNullOrEmpty(hrefDisplay.Content))
                            hrefDisplay.Content = content;
                    }
                }
            }
            return fieldInfo;
        }

        private static string CreateContent(IInputData input, IDisplayObject dataXml)
        {
            return string.Format(ObjectUtil.SysCulture, "~/c/xml/detail/{0}?{1}=*{1}*",
                input.SourceInfo.Source, dataXml.Id.NickName);
        }

        public static bool IsShowInList(IFieldControl control, Tk5ListDetailConfig listDetail,
            FieldKind kind, IPageStyle style, bool isInTable)
        {
            if (control == null)
                return false;
            if ((control.DefaultShow & PageStyle.List) != PageStyle.List
                || kind != FieldKind.Data)
                return false;

            if (isInTable)
            {
                var ctrl = control.GetControl(style);
                if (ctrl == ControlType.Hidden)
                    return false;
                if (listDetail != null && listDetail.Search == FieldSearchMethod.Only)
                    return false;
            }

            return true;
        }

        public static IMetaData CreateVueMetaData(IInputData input, ITableSchemeEx scheme, ISingleMetaData singleMeta)
        {
            InputDataProxy proxy;
            switch (input.Style.Operation)
            {
                case "ListVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                    return new Tk5ListMetaData(scheme, proxy, singleMeta);

                case "InsertVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Insert);
                    return new Tk5SingleNormalMetaData(scheme, proxy, singleMeta);

                case "UpdateVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Update);
                    return new Tk5SingleNormalMetaData(scheme, proxy, singleMeta);

                case "DetailVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Detail);
                    return new Tk5SingleNormalMetaData(scheme, proxy, singleMeta);
            }

            return null;
        }

        private static Tk5MultipleMetaData CreateMultipleMetaData(IInputData input, BaseMDSingleMetaDataConfig master,
            IEnumerable<DetailSingleMetaDataConfig> details)
        {
            ISingleMetaData masterMeta = master.CreateSingleMetaData();
            return new Tk5MultipleMetaData(input, EnumUtil.Convert(masterMeta), details);
        }

        public static IMetaData CreateVueMetaData(IInputData input, BaseMDSingleMetaDataConfig master,
            IEnumerable<DetailSingleMetaDataConfig> details)
        {
            InputDataProxy proxy;
            switch (input.Style.Operation)
            {
                case "ListVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                    ISingleMetaData masterMeta = master.CreateSingleMetaData();
                    var scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, proxy, masterMeta);

                case "InsertVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Insert);
                    return CreateMultipleMetaData(proxy, master, details);

                case "UpdateVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Update);
                    return CreateMultipleMetaData(proxy, master, details);

                case "DetailVue":
                    proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.Detail);
                    return CreateMultipleMetaData(proxy, master, details);
            }

            return null;
        }
    }
}