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
        private static string RemoveSpecialChars(string line)
        {
            foreach (string s in TeX.EscapedChars.Keys)
            {
                line = line.Replace(s, TeX.EscapedChars[s]);
            }

            return line.Replace('`', '\'').Replace("''", "\"");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="doc"></param>
        public static void AddParagraph(List<List<string>> lines, DocX doc)
        {
            Paragraph p = doc.InsertParagraph();
            List<List<string>> l = lines;

            foreach (List<string> ss in l)
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

                Append(p, temps[0]);

                for (int i = 1; i < ss.Count; i++)
                {
                    string pattern = @"\\(?<type>[a-zA-Z0-9]+)(?:\{(?<output>[^\{\}]+)\})?(?:\{(?<output2>[^\{\}]+)\})?";
                    Match m = Regex.Match(ss[i], pattern);
                    switch (m.Groups["type"].Value)
                    {
                        case "textbf":

                            Append(p, m.Groups["output"].Value);
                            p.Bold();

                            p.Append(temps[i]);
                            break;

                        case "emph":
                        case "textit":
                            Append(p, m.Groups["output"].Value);
                            p.Italic();
                            Append(p, temps[i]);
                            break;

                        case "rlap":
                            Append(p, m.Groups["output"].Value);
                            Append(p, m.Groups["output2"].Value);
                            break;

                        default:
                            throw new Exception(m.Groups["type"] + " is still under development");
                    }
                }

                string blah = doc.Xml.Value;
            }
        }

        /// <summary>
        /// Appends text to p removing special characters as it goes
        /// </summary>
        /// <param name="p"></param>
        /// <param name="text"></param>
        private static void Append(Paragraph p, string text)
        {
            p.Append(RemoveSpecialChars(text));
        }
    }
}