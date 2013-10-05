using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace TexWordCompiler
{
    [TestFixture]
    public class Test
    {
        private string TempOutput = "C:\\Users\\Tony\\Desktop\\TexWordCompilerTestFiles\\Test";

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

            FileInfo f = new FileInfo(String.Format("{0}Test.Tex", dir.FullName));

            //f.Create();

            using (StreamWriter w = new StreamWriter(f.FullName))
            {
                w.WriteLine(@"an existing residential building. \emph{Building regulations part L2B} detail the measures ");
                w.WriteLine(@"%comment line");
                w.WriteLine(@"\thing{new-build development}{potato}");
                w.WriteLine(@"\thing{trees}{with}[optional Monkeys]");

            }

            using (StreamReader sr = new StreamReader(f.FullName))
            {
                Dictionary<int, List<string>> outputs = new Dictionary<int, List<string>>();
                Dictionary<int, List<string>> type = new Dictionary<int, List<string>>();
                Dictionary<int, List<string>> parameters = new Dictionary<int, List<string>>();
                Dictionary<int, List<string>> optionals = new Dictionary<int, List<string>>();


                while (!sr.EndOfStream)
                {
                    r.GetNextBlock(sr);

                }
            }
            f.Delete();
        }

        //[Test]
        //public void MakeSimpleDocument()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.WordDocument w = new OutputFiles.WordDocument(dir);

        //    w.AddLine("test Line");

        //    w.End();
        //    //using (OutputFiles.FontTable f = new OutputFiles.FontTable(dir)) ;
        //    //using (OutputFiles.WebSettings w = new OutputFiles.WebSettings(dir)) ;
        //}

        //[Test]
        //public void MakeFontTable()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.FontTable f = new OutputFiles.FontTable(dir);
        //}

        //[Test]
        //public void MakeWebSettings()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.WebSettings w = new OutputFiles.WebSettings(dir);
        //}

        //[Test]
        //public void MakeSettings()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.Settings w = new OutputFiles.Settings(dir);
        //}

        //[Test]
        //public void MakeApp()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.App w = new OutputFiles.App(dir);
        //}

        //[Test]
        //public void MakeCore()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.Core w = new OutputFiles.Core(dir);
        //}

        //[Test]
        //public void MakeStyle()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.Styles w = new OutputFiles.Styles(dir);
        //}

        //[Test]
        //public void MakeRels()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.Rels w = new OutputFiles.Rels(dir);
        //}

        //[Test]
        //public void MakeContentType()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);

        //    Dictionary<string, string> defaults = new Dictionary<string, string>();
        //    defaults.Add("rels", "application/vnd.openxmlformats-package.relationships+xml");
        //    defaults.Add("xml", "application/xml");

        //    Dictionary<string, string> partNames = new Dictionary<string, string>();

        //    partNames.Add("/word/document.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
        //    partNames.Add("/word/styles.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml");
        //    partNames.Add("/docProps/app.xml", "application/vnd.openxmlformats-officedocument.extended-properties+xml");
        //    partNames.Add("/word/settings.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.settings+xml");
        //    partNames.Add("/word/theme/theme1.xml", "application/vnd.openxmlformats-officedocument.theme+xml");
        //    partNames.Add("/word/fontTable.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml");
        //    partNames.Add("/word/webSettings.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.webSettings+xml");
        //    partNames.Add("/docProps/core.xml", "application/vnd.openxmlformats-package.core-properties+xml");

        //    OutputFiles.ContentType f = new OutputFiles.ContentType(dir, defaults, partNames);
        //}

        //[Test]
        //public void MakeDocumentRels()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);

        //    OutputFiles.documentXmlRels f = new OutputFiles.documentXmlRels(dir);
        //}

        //[Test]
        //public void MakeTheme()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(TempOutput);
        //    OutputFiles.Theme w = new OutputFiles.Theme(dir);
        //}

        //[Test]
        //public void MakeTestOutput()
        //{
        //    MakeFontTable();
        //    MakeTheme();
        //    MakeApp();
        //    MakeCore();
        //    MakeSettings();
        //    MakeWebSettings();
        //    MakeStyle();
        //    MakeRels();
        //    MakeContentType();
        //    MakeDocumentRels();

        //    Zip();
        //}

        [Test]
        public void InteropTest()
        {
            OutputFiles.Output sh = new OutputFiles.Output();
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


            foreach(DirectoryInfo d in dirs)
            {
                z.AddFolder(d);
            }

            z.Zip();
        }
    }
}