using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace TexWordCompiler
{
    [TestFixture]
    public class Test
    {
        //private string TempOutput = "C:\\Users\\Tony\\Desktop\\TexWordCompilerTestFiles\\Test";
        private string TempOutput = "C:\\Users\\TBarnett\\Desktop\\TexWordCompilerTestFiles\\Test";

        [Test]
        public void Parser()
        {
            //Reader.Line r = new Reader.Line();

            //r.ParseLine(@"\thing{bananas \test{grow} on \test } \otherThing{trees}");

            List<string> output = new List<string>();
            output.Add(@"{1} {3}");
            output.Add(@"bananas {2} on test ");
            output.Add(@"grow");
            output.Add(@"trees");

            List<string> type = new List<string>();
            type.Add("thing");
            type.Add("test");
            type.Add("otherThing");

            //Assert.AreEqual(r.Output, output);
            //Assert.AreEqual(r.Type, type);
        }

        [Test]
        public void LineIn()
        {
            string line = "let's get TeXy with \\things and \\textbf{otherThings} % Complete with commenty goodness \\% % ahoyhoy";

            List<string> output = TeX.ReadLine(line);
        }

        [Test]
        public void ReplaceTex()
        {
            List<string> tex = new List<string>();
            tex.Add(@"This is {1} line with a citation {2}");
            tex.Add(@"\\textbf{a tex}");
            tex.Add(@"\\cite{person}");

            for (int i = 1; i < tex.Count; i++)
            {
                string output = TeX.ReplaceTeX(tex[i]);
            }
        }

        [Test]
        public void Zip()
        {
            //DirectoryInfo dir = new DirectoryInfo(@"C:\\Users\\Tony\\Desktop\\test_docx");
            DirectoryInfo dir = new DirectoryInfo(TempOutput);
            DirectoryInfo outputDir = new DirectoryInfo(TempOutput);
            ZipFiles z = new ZipFiles(outputDir);

            List<DirectoryInfo> dirs = new List<DirectoryInfo>();
            dirs.Add(new DirectoryInfo(dir + "\\_rels"));
            dirs.Add(new DirectoryInfo(dir + "\\docProps"));
            dirs.Add(new DirectoryInfo(dir + "\\word"));

            FileInfo f = new FileInfo(dir.FullName + @"\\[Content_Types].xml");

            z.AddFile(f);

            foreach (DirectoryInfo d in dirs)
            {
                z.AddFolder(d);
            }

            z.Zip();
        }

        [Test]
        public void EscapedCommentChar()
        {
            List<string> line = TeX.ReadLine(@"This is a line with an escaped \% in it. % not kidding");
            List<string> correctLine = new List<string>();

            correctLine.Add(@"This is a line with an escaped % in it. ");

            Assert.AreEqual(line, correctLine);
        }

        [Test]
        public void AddParagraph()
        {
            Novacode.DocX d = Novacode.DocX.Create("thing.docx");
            List<List<string>> p = new List<List<string>>();

            p.Add(new List<string>());
            p[0].Add("let's get TeXy with {1} and {2}");
            p[0].Add("\\textbf{Bold bits}");
            p[0].Add("\\textit{italic bits}");
            WordifyThings.AddParagraph(p, d);

            d.Save();
        }
    }
}