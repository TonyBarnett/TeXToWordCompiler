using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class WebSettings
    {
        public WebSettings(DirectoryInfo doc)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            namespaces.Add("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            OutputFile f = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\word\\webSettings.xml"), "webSettings", "w");

            f.Add(new Xml.Document("w:optimizeForBrowser"));

            f.Done();
        }
    }
}