using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Novacode;
using System.Text.RegularExpressions;
using System.IO;

namespace TexWordCompiler
{
    class Output
    {
        private string _OutputName;
        private string _InputName;

        public Output(string inputName, string outputName)
        {
            _OutputName = outputName;
            _InputName = inputName;
        }

        public void Run()
        {
            using (DocX o = DocX.Create(string.Format("{0}\\.Docx", _OutputName)))
            {
                using (Reader r = new Reader(_InputName))
                {
                    r.GetHeaderInformation();
                    // TODO: do something with said header information that 
                    // has been so lovingly prepared.
                    Reader.Line l;
                    StringBuilder sb = new StringBuilder();
                    int wordCount = 0; // use me to work out which word in the line needs
                        // special treatment?
                    while (!r.EndOfStream)
                    {
                        l = r.ParseLine();

                        sb.Append(l.ThisLine);

                        if (l.Italics!= null && l.Italics.Count > 0)
                        {
                            // Tell output that words need to be in italics
                        }

                        if (l.Bold!= null && l.Bold.Count > 0)
                        {
                            // Tell output that words need to be in bold
                        }

                        if (l.EndParagraph)
                        {
                            o.InsertParagraph(sb.ToString());
                        }
                    }
                }

                o.SaveAs(_OutputName);
            }
        }
    }
}