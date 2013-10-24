﻿using System;
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