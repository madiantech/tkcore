using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    public class DateStruct
    {
        public DateStruct()
        {
        }

        public DateStruct(DateTime date)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        [SimpleElement(LocalName = "年")]
        public int Year { get; set; }

        [SimpleElement(LocalName = "月")]
        public int Month { get; set; }

        [SimpleElement(LocalName = "日")]
        public int Day { get; set; }
    }
}