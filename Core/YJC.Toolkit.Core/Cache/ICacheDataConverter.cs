using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Cache
{
    public interface ICacheDataConverter
    {
        byte[] Convert(object data);

        object CreateEmptyData();

        object ReadData(byte[] data);
    }
}