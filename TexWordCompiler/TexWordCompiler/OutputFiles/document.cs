using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xml;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class WordDocument
    {
        private OutputFile _F;
        private Document _Body;

        private Document _Paragraph;
        /// <summary>
        /// starts a Document in "doc" and puts all namespace rubbish 
        /// at the beginning
        /// <param name="doc"></param>
        /// </summary>
        public WordDocument(DirectoryInfo doc)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();


            namespaces.Add("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            namespaces.Add("o", "urn:schemas-microsoft-com:office:office");
            namespaces.Add("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            namespaces.Add("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            namespaces.Add("v", "urn:schemas-microsoft-com:vml");
            namespaces.Add("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            namespaces.Add("w10", "urn:schemas-microsoft-com:office:word");
            namespaces.Add("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            namespaces.Add("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            
            
            //Write("<w:background w:color=\"FFFFFF\"/><w:body>");
            //Write("<w:body>");
            _F = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\word\\document.xml"), "document", "w");

            _Body = new Document("w:body");

            _Paragraph = new Document("w:p");
            _Paragraph.AddAttribute("w:rsidR", "00000000");
            _Paragraph.AddAttribute("w:rsidRDefault", "00000000");
            _Paragraph.AddAttribute("w:rsidP", "00000000");
        }

        public void NextParagrah()
        {
            
            _Body.Add(_Paragraph);
            _Paragraph = new Document("w:p");
            _Paragraph.AddAttribute("w:rsidR", "00000000");
            _Paragraph.AddAttribute("w:rsidRDefault", "00000000");
            _Paragraph.AddAttribute("w:rsidP", "00000000");
        }

        public void AddLine(string line)
        {
            Document t = new Document("w:t","xml:space","00000000");
            t.SetValue(line);
            _Paragraph.Add(t);
        }

        /// <summary>
        /// Don't forget to call me when you're done.
        /// </summary>
        public void End()
        {
            _Body.Add(_Paragraph);

            Document endGoop = new Document("w:sectPr");
            Dictionary <string, string> attributes = new Dictionary<string,string>();

            attributes.Add("w:w", "12240");
            attributes.Add( "w:h", "15840");
            endGoop.Add(new Document("w:pgSz", attributes));

            attributes = new Dictionary<string,string>();
            attributes.Add("w:left", "1440");
            attributes.Add( "w:right","1440");
            attributes.Add( "w:top", "1440");
            attributes.Add("w:bottom","1440");

            endGoop.Add(new Document("w:pgMar", attributes));

            _Body.Add(endGoop);

            _F.Add(_Body);

            _F.Done();
        }
    }
}