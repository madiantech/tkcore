using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class PageInfoSource : ISource, ISupportMetaData
    {
        private IMetaData fMetaData;

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion ISupportMetaData 成员

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            var style = input.Style.Style;
            var url = string.Format(ObjectUtil.SysCulture, "c/xml/{0}/{1}?", style, input.SourceInfo.Source);
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.ReadQueryString(input.QueryStringText);
            query["Type"] = "Data";
            string dataUrl = url + query.WriteQueryString();
            query["Type"] = "Frame";
            string frameUrl = url + query.WriteQueryString();

            string tableName;
            if (fMetaData is INormalMetaData)
                tableName = ((INormalMetaData)fMetaData).TableData.TableName;
            else
                tableName = string.Empty;
            string pageType;
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    pageType = "edit";
                    break;

                case PageStyle.Detail:
                    pageType = "detail";
                    break;

                case PageStyle.List:
                    pageType = "list";
                    break;

                default:
                    pageType = "other";
                    break;
            }
            var result = new PageUrlInfo
            {
                Title = fMetaData.Title,
                Width = "50%",
                Query = new PageTemplateInfo
                {
                    PageType = pageType,
                    DataUrl = dataUrl,
                    TempUrl = frameUrl,
                    DataConf = new TableStructure
                    {
                        Structure = tableName
                    }
                }
            };

            return OutputData.CreateToolkitObject(result);
        }

        #endregion ISource 成员
    }
}