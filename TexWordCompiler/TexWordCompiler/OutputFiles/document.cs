using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xml;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class WordDocument : StreamWriter
    {
        private Document _Paragraph;
        /// <summary>
        /// starts a Document in "doc" and puts all namespace rubbish 
        /// at the beginning
        /// <param name="doc">directory where this wants to go within structure</param>
        /// </summary>
        public WordDocument(DirectoryInfo doc)
            : base(doc.FullName + "\\word\\document.xml")
        {
            WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            Write("<w:document ");
            Write("xmlns:ve=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" ");
            Write("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
            Write("xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" ");
            Write("xmlns:m=\"http://schemas.openxmlformats.org/officeDocument/2006/math\" ");
            Write("xmlns:v=\"urn:schemas-microsoft-com:vml\" ");
            Write("xmlns:wp=\"http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing\" ");
            Write("xmlns:w10=\"urn:schemas-microsoft-com:office:word\" ");
            Write("xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\" ");
            Write("xmlns:wne=\"http://schemas.microsoft.com/office/word/2006/wordml\"");
            Write(">");
            //Write("<w:background w:color=\"FFFFFF\"/><w:body>");
            Write("<w:body>");

            _Paragraph = new Document("w:p");
            _Paragraph.AddAttribute("w:rsidR", "00000000");
            _Paragraph.AddAttribute("w:rsidRDefault", "00000000");
            _Paragraph.AddAttribute("w:rsidP", "00000000");
        }

        public void NextParagrah()
        {
            Write(_Paragraph.GetXml());
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
            Write(_Paragraph.GetXml());
            Write("<w:sectPr><w:pgSz w:w=\"12240\" w:h=\"15840\"/><w:pgMar w:left=\"1440\" w:right=\"1440\" w:top=\"1440\" w:bottom=\"1440\"/></w:sectPr></w:body>");

            WriteLine("</w:document>");
        }
    }
}