using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal static class TestLinq
    {
        public static void Group()
        {
            int[] data = new int[] { 1, 2, 4, 5, 6, 8, 9, 10, 11, 13, 14, 15 };
            List<(int min, int max)> groups = new List<(int min, int max)>() { (1, 5), (6, 7), (8, 12), (13, 15), (16, 20) };

            var result = from grp in groups
                         from intData in data
                         where intData >= grp.min && intData <= grp.max
                         group intData by grp;
            foreach (var item in result)
            {
                var grp = ((int min, int max))item.Key;
                Console.Write($"Min:{grp.min}, Max:{grp.max}: ");
                foreach (var value in item)
                    Console.Write($"{value} ");
                Console.WriteLine();
            }
        }
    }
}