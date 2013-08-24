using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    class Styles 
    {
        public Styles(DirectoryInfo doc)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("o", "urn:schemas-microsoft-com:office:office");
            namespaces.Add("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            namespaces.Add("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            namespaces.Add("v", "urn:schemas-microsoft-com:vml");
            namespaces.Add("w10", "urn:schemas-microsoft-com:office:word");
            namespaces.Add("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            namespaces.Add("sl", "http://schemas.openxmlformats.org/schemaLibrary/2006/main");

            Document docDefaults = new Document("w:docDefaults");
            Document rPrDefault = new Document("w:rPrDefault");
            Document rPr = new Document("w:rPr");


        }
    }
}
