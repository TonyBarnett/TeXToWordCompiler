using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TexWordCompiler.OutputFiles
{
    class Rels
    {
        public Rels(DirectoryInfo doc)
        {
            OutputFile f = new OutputFile(new Dictionary<string, string>(), new FileInfo(doc.FullName + "\\_rels\\.rels"), "Relationships", "");
            
            f.AddAttribute("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");

            Dictionary<string,string> attributes = new Dictionary<string,string>();
            attributes.Add("Id", "rId3");
            attributes.Add("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
            attributes.Add("Target", "docProps/app.xml");
            f.Add(new Xml.Document("Relationship",attributes));
            
            attributes["Id"] = "rId2";
            attributes["Type"] = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties";
            attributes["Target"] = "docProps/core.xml";
            f.Add(new Xml.Document("Relationship",attributes));

            attributes["Id"] = "rId1";
            attributes["Type"] = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
            attributes["Target"] = "word/document.xml";
            f.Add(new Xml.Document("Relationship",attributes));
	
            f.Done();
        }
    }
}
