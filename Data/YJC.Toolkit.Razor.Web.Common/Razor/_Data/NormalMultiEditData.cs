using System;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalMultiEditData : NormalEditData, ITableDataIndexer
    {
        private readonly RegNameList<SingleTableEditData> fTableDatas;

        public NormalMultiEditData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalMultiEditData));
            if (defaultCreator != null && DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                this.CopyFromObject(defaultObject);
            }
            fTableDatas = new RegNameList<SingleTableEditData>();
        }

        internal NormalMultiEditData(NormalMultiEditDataConfig config)
            : base(config)
        {
            fTableDatas = new RegNameList<SingleTableEditData>();
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

        public SingleTableEditData this[string tableName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

                return fTableDatas[tableName];
            }
        }

        SingleTableDetailData ITableDataIndexer.this[string tableName] { get => this[tableName]; }

        public void Add(SingleTableEditData tableData)
        {
            TkDebug.AssertArgumentNull(tableData, "tableData", this);

            fTableDatas.Add(tableData);
            tableData.Initialize();
        }
    }
}