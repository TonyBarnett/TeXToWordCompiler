using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Novacode;

namespace TexWordCompiler
{
    public static class WordifyThings
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="doc"></param>
        public static void AddParagraph(List<List<string>> lines, DocX doc)
        {
            Paragraph p = doc.InsertParagraph();

            foreach (List<string> ss in lines)
            {
                MatchCollection mc = Regex.Matches(ss[0], @"\{[0-9]+\}");
                string temp = ss[0];
                foreach (Match m in mc)
                {
                    foreach (Capture c in m.Captures)
                    {
                        temp = temp.Replace(c.Value, "¬");
                    }
                }

                string[] temps = temp.Split('¬');

                p.Append(temps[0]);
                //doc.Paragraphs.Add(p);
                for (int i = 1; i < ss.Count; i++)
                {
                    string pattern = @"\\(?<type>[a-zA-Z0-9]+)(\{(?<output>[a-zA-Z0-9 ]+)\})?";
                    Match m = Regex.Match(ss[i], pattern);
                    switch (m.Groups["type"].Value)
                    {
                        case "textbf":

                            p.Append(m.Groups["output"].Value).Bold();

                            p.Append(temps[i]);
                            break;

                        case "textit":
                            p.Append(m.Groups["output"].Value).Italic();

                            p.Append(temps[i]);
                            break;
                    }
                }

                string blah = doc.Xml.Value;
            }
        }
    }
}