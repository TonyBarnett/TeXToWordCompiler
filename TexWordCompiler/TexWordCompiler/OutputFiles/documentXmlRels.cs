using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    class documentXmlRels
    {
        public documentXmlRels(DirectoryInfo doc)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            Document relationships = new Document("Relationships", "xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
            attributes.Add("Id", "rId1");
            attributes.Add("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
            attributes.Add("Target", "styles.xml");
            relationships.Add(new Document("Relationship", attributes));

            attributes["Id"] = "rId2";
            attributes["Type"] = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/settings";
            attributes["Target"] = "settings.xml";
            relationships.Add(new Document("Relationship", attributes));

            attributes["Id"] = "rId3";
            attributes["Type"] = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/webSettings";
            attributes["Target"] = "webSettings.xml";
            relationships.Add(new Document("Relationship", attributes));

            attributes["Id"] = "rId4";
            attributes["Type"] = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/fontTable";
            attributes["Target"] = "fontTable.xml";
            relationships.Add(new Document("Relationship", attributes));

            attributes["Id"] = "rId5";
            attributes["Type"] = "theme/theme1.xml";
            attributes["Target"] = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme";
            relationships.Add(new Document("Relationship", attributes));

            FileInfo f = new FileInfo(doc.FullName + "\\word\\_rels\\document.xml.rels");
            if (!f.Exists)
            {
                if (!f.Directory.Exists)
                {
                    f.Directory.Create();
                }
                //f.Create();
            }

            using (StreamWriter w = new StreamWriter(f.FullName))
            {
                w.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                w.WriteLine(relationships.GetXml());
            }
        }
    }
}