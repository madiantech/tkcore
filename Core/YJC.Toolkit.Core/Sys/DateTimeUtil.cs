using System;

namespace YJC.Toolkit.Sys
{
    public static class DateTimeUtil
    {
        public static int CalAge(DateTime today, DateTime birth)
        {
            int age = today.Year - birth.Year;
            if (today.Month < birth.Month)
                return --age;
            if (today.Month == birth.Month && today.Day < birth.Day)
                return --age;
            return age;
        }
    }
}
