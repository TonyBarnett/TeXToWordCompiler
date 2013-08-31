using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xml;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class ContentType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaults">from the output <Default Extension="key" ContentType="value"/> </param>
        /// <param name="overrides">from the output <Default PartName="key" ContentType="value"/></param>
        public ContentType(DirectoryInfo doc, Dictionary<string, string> defaults, Dictionary<string, string> overrides)
        {
            Document output = new Document("Types", "xmlns", "http://schemas.openxmlformats.org/package/2006/content-types");

            foreach (string s in defaults.Keys)
            {
                Dictionary<string,string> namespaces = new Dictionary<string,string>();
                namespaces.Add("Extension", s);
                namespaces.Add("ContentType", defaults[s]);

                output.Add(new Document("Default", namespaces));
            }

            foreach (string s in defaults.Keys)
            {
                Dictionary<string,string> namespaces = new Dictionary<string,string>();
                namespaces.Add("PartName", s);
                namespaces.Add("ContentType", defaults[s]);

                output.Add(new Document("Override", namespaces));
            }

            FileInfo outputFile = new FileInfo(doc.FullName + "\\[Content_Types].xml");

            using (StreamWriter w = new StreamWriter(outputFile.FullName))
            {
                if (!outputFile.Exists)
                {
                    outputFile.Create();
                }

                w.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                w.WriteLine(output.GetXml());
            }
        }
    }
}
