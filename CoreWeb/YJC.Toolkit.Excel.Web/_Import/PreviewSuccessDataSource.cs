using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class PreviewSuccessDataSource : EmptyDbDataSource, ISource
    {
        private readonly IConfigCreator<TableResolver> fCreator;

        public PreviewSuccessDataSource(IConfigCreator<TableResolver> creator)
        {
            fCreator = creator;
        }

        public OutputData DoAction(IInputData input)
        {
            ImportResultData result = ImportUtil.GetResultData(input);

            MetaDataTableResolver resolver = fCreator.CreateObject(
                new TempSource(result.ImportDataSet, this)).Convert<MetaDataTableResolver>();
            resolver.Decode((PageStyleClass)PageStyle.List);

            return OutputData.Create(result.ImportDataSet);
        }
    }
}