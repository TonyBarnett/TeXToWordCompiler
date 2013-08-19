using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TexWordCompiler.OutputFiles
{
    class Core
    {
        public Core(DirectoryInfo doc)
        {

            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("cp", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
            namespaces.Add("dc", "http://purl.org/dc/elements/1.1/");
            namespaces.Add("dcterms", "http://purl.org/dc/terms/");
            namespaces.Add("dcmitype", "http://purl.org/dc/dcmitype/");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            OutputFile f = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\docProps\\core.xml"), "coreProperties", "cp");

            f.Add(new Xml.Document("dc:creator", "tony.barnett"));
            f.Add(new Xml.Document("cp:lastModifiedBy", "tony.barnett"));
            f.Add(new Xml.Document("cp:revision", "1"));
            f.Add(new Xml.Document("dcterms:created", "xsi:type", "dcterms:W3CDTF", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ")));
            f.Add(new Xml.Document("dcterms:modified", "xsi:type", "dcterms:W3CDTF", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ")));

            f.Done();
        }
    }
}