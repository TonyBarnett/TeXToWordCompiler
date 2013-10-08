using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class Output
    {
        private Document _Document;
        Paragraph _Paragraph;

        public Output()
        {
            _Document = new Document(); // Create a document
            _Paragraph = _Document.Content.Paragraphs.Add(); // Create a paragraph as added to the document.
        }

        /// <summary>
        /// Next paragraph.
        /// </summary>
        public void NewParagragh()
        {
            _Paragraph = _Document.Content.Paragraphs.Add(); // Create a paragraph as added to the document.
        }

        /// <summary>
        /// Add a line to a paragraph
        /// </summary>
        /// <param name="text">Line of text.</param>
        public void AddLine(string text)
        {
            _Paragraph.Range.Text += string.Format("{0}\n", text);
        }

        public void Save(string where)
        {
            string thing = where + "\\thing.docx";
            _Document.SaveAs(thing, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument);
        }
    }
}