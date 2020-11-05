using System;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalMultiDetailData : NormalDetailData, ITableDataIndexer
    {
        private readonly RegNameList<SingleTableDetailData> fTableDatas;

        public NormalMultiDetailData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalMultiDetailData));
            if (defaultCreator != null && DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                this.CopyFromObject(defaultObject);
            }
            fTableDatas = new RegNameList<SingleTableDetailData>();
        }

        internal NormalMultiDetailData(NormalMultiDetailDataConfig config)
            : base(config)
        {
            fTableDatas = new RegNameList<SingleTableDetailData>();
            if (config.TableDatas != null)
                foreach (var tableData in config.TableDatas)
                {
                    Add(tableData);
                    if (tableData.RazorFields != null)
                    {
                        foreach (var razorField in tableData.RazorFields)
                        {
                            string key = GetKey(tableData.TableName, razorField.NickName);
                            AddDisplayField(new RazorField(key, razorField.ContentType, razorField.Content));
                        }
                    }
                }
            Header = config.Header;
            Footer = config.Footer;
        }

        public SingleTableDetailData this[string tableName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

                return fTableDatas[tableName];
            }
        }

        public void Add(SingleTableDetailData tableData)
        {
            TkDebug.AssertArgumentNull(tableData, "tableData", this);

            fTableDatas.Add(tableData);
            tableData.Initialize();
        }
    }
}