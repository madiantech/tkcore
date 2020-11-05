using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [JsonObjectPageMaker]
    [JsonObjectCreator(typeof(EasySearchInput))]
    [Source(Author = "YJC", CreateDate = "2014-08-21",
        Description = "提供EasySearch下拉搜索时的数据")]
    [WebPage(SupportLogOn = false)]
    internal class EasySearchSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            if (input.IsPost)
            {
                EasySearchInput esIn = input.PostObject.Convert<EasySearchInput>();
                EasySearch easySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                    EasySearchPlugInFactory.REG_NAME, esIn.RegName);
                using (easySearch as IDisposable)
                {
                    IEnumerable<IDecoderItem> result = easySearch.Search(esIn.Text, esIn.RefFields);

                    //EasySearchOutput output = new EasySearchOutput(result);
                    IDecoderItem[] resultArr;
                    if (result != null)
                        resultArr = result.ToArray();
                    else
                        resultArr = new CodeItem[0];
                    return OutputData.CreateToolkitObject(resultArr);
                }
            }

            TkDebug.ThrowImpossibleCode(this);
            return null;
        }

        #endregion
    }
}
