using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TexWordCompiler
{
    [TestFixture]
    public class Test
    {
        public Test()
        {
            Reader.Line r = new Reader.Line();

            r.ParseLine(@"\\thing{bananas \\test{grow} on \\test } \\otherThing{trees}");
            
            List<string> output = new List<string>();
            output.Add(@"{1} {3}");
            output.Add(@"bananas {2} on \test ");
            output.Add(@"bananas {2} on \test ");
            output.Add(@"trees");

            List<string> type = new List<string>();
            type.Add("thing");
            type.Add("test");
            type.Add("otherThing");

            Assert.AreEqual(r.Output, output);
            Assert.AreEqual(r.Type, type);
        }

    }
}
