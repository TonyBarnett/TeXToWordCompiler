using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace TexWordCompiler
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void Parser()
        {
            Reader.Line r = new Reader.Line();

            r.ParseLine(@"\thing{bananas \test{grow} on \test } \otherThing{trees}");

            List<string> output = new List<string>();
            output.Add(@"{1} {3}");
            output.Add(@"bananas {2} on test ");
            output.Add(@"grow");
            output.Add(@"trees");

            List<string> type = new List<string>();
            type.Add("thing");
            type.Add("test");
            type.Add("otherThing");

            Assert.AreEqual(r.Output, output);
            Assert.AreEqual(r.Type, type);
        }

        [Test]
        public void LineIn() // finish me... I got too tired and a little bored last time
        {
            Reader.Line r = new Reader.Line();

            DirectoryInfo dir = new DirectoryInfo("..\\");

            FileInfo f = new FileInfo(String.Format("{0}Test.Tex",dir.FullName));

            //f.Create();

            using (StreamWriter w = new StreamWriter(f.FullName))
            {
                w.WriteLine(@"an existing residential building. \emph{Building regulations part L2B} detail the measures ");
                w.WriteLine(@"\section*{b: the ");
                w.WriteLine(@"%comment line");
                w.WriteLine(@"new-build development}");
                w.WriteLine(@"\thing{new-build development}{potato}");
                w.WriteLine(@"\thing{trees}{with}[optional Monkeys]");

            }

            StreamReader sr = new StreamReader(f.FullName);

            List<string> output = new List<string>();
            List<string> type = new List<string>();
            List<string> optional = new List<string>();

            while (!sr.EndOfStream)
            {
                r.GetNextBlock(sr);

                
            }

            f.Delete();
        }

    }
}
