using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    class Styles : StreamWriter
    {
        public Styles(DirectoryInfo doc)
            : base(doc.FullName + "\\word\\webSettings.xml")
        {
            Write("<w:settings");
            Write("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            Write("xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"");
            Write("xmlns:m=\"http://schemas.openxmlformats.org/officeDocument/2006/math\"");
            Write("xmlns:v=\"urn:schemas-microsoft-com:vml\"");
            Write("xmlns:w10=\"urn:schemas-microsoft-com:office:word\"");
            Write("xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\"");
            Write("xmlns:sl=\"http://schemas.openxmlformats.org/schemaLibrary/2006/main\">");

            Document docDefaults = new Document("w:docDefaults");
            Document rPrDefault = new Document("w:rPrDefault");
            Document rPr = new Document("w:rPr");


        }
    }
}
