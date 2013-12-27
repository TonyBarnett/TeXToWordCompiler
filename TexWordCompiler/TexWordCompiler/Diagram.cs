using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TexWordCompiler
{
    internal class Diagram
    {
        private static int _Number = 0;

        /// <summary>
        /// The figure number.
        /// </summary>
        public int Number;

        public List<string> Label;

        public List<FileInfo> Files;

        /// <summary>
        /// we want to be able to handle both differing captions
        /// for figures and list of figures, as well as a caption for
        /// multiple subFigures.
        /// </summary>
        public Dictionary<string, List<string>> Caption;

        /// <summary>
        /// What I'm thinking is that you pass the entire
        /// \begin{thing} to \end{thing} and it'll work
        /// everything else out from that. We can handle
        /// subfigures as well.
        /// </summary>
        /// <param name="teX"></param>
        public Diagram(string teX)
        {
            // We need to know which figure number we are.
            Number = ++_Number;
            Files = new List<FileInfo>();
            Caption = new Dictionary<string, List<string>>();
            Label = new List<string>();
            string line = teX.Replace("\n", "").Replace("\r", "");
            MatchCollection mc = Regex.Matches(teX.Replace("\n", "").Replace("\r", ""), TeX.RegexLine);
            foreach (Match m in mc)
            {
                string t = m.Groups["type"].ToString().Trim();
                string v = m.Groups["value"].ToString();
                string o = m.Groups["optional"].ToString();
                switch (m.Groups["type"].ToString().Trim())
                {
                    case "includegraphics":
                        Files.Add(new FileInfo(m.Groups["value"].ToString()));
                        break;

                    case "caption":
                        string c = Regex.Replace(m.Groups["value"].ToString(), "\\s+", " ");
                        Caption.Add(c, new List<string>());
                        if (m.Groups["optional"].Success)
                        {
                            Caption[c].Add(m.Groups["optional"].ToString());
                        }
                        break;

                    case "label":
                        Label.Add(m.Groups["value"].ToString());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}