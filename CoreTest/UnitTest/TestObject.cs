using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Sys;

namespace UnitTest
{
    public class TestObject
    {
        [SimpleAttribute]
        public int Hello { get; set; }

        [SimpleElement]
        [NameModel("Test", LocalName = "PWorld")]
        public string World { get; set; }

        //[TextContent]
        //public double Content { get; set; }

        [SimpleAttribute]
        public ConsoleModifiers? Modifiers { get; set; }
    }
}