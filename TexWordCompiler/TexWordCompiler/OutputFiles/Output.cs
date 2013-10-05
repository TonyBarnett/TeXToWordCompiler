using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace TexWordCompiler.OutputFiles
{
    class Output
    {
        public Output()
        {
            Document d = new Document(); // Create a document
            Paragraph p = d.Content.Paragraphs.Add(); // Create a paragraph as addrd to the document.
            p.Range.Text = "eragsat"; // Add text to the paragraph
            string funky = d.WordOpenXML; // read out the xml, might be a better idea to read this out to a 
                // streamWriter to form the word 2007 format
            //d.Save(); // save thge document
        }
    }
}
