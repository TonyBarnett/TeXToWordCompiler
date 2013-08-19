using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TexWordCompiler.OutputFiles
{
    class App
    {
        public App(DirectoryInfo doc)
        {
            OutputFile f = new OutputFile(new Dictionary<string, string>(), new FileInfo(doc.FullName + "\\docProps\\app.xml"), "Properties", "");

            f.AddAttribute("xmlns", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties");
            f.AddAttribute("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");

            f.Add(new Xml.Document("Template", "Normal.dotm"));
            f.Add(new Xml.Document("TotalTime", "1"));
            f.Add(new Xml.Document("Pages", "1"));
            f.Add(new Xml.Document("Words", "0"));
            f.Add(new Xml.Document("Characters", "4"));
            f.Add(new Xml.Document("Application", "Microsoft Office Word"));
            f.Add(new Xml.Document("DocSecurity", "0"));
            f.Add(new Xml.Document("Lines", "1"));
            f.Add(new Xml.Document("Paragraphs", "1"));
            f.Add(new Xml.Document("ScaleCrop", "false"));
            f.Add(new Xml.Document("Company", "T"));
            f.Add(new Xml.Document("LinksUpToDate", "false"));
            f.Add(new Xml.Document("CharactersWithSpaces", "4"));
            f.Add(new Xml.Document("SharedDoc", "false"));
            f.Add(new Xml.Document("HyperlinksChanged", "false"));
            f.Add(new Xml.Document("AppVersion", "12.0000"));

            f.Done();
        }
    }
}