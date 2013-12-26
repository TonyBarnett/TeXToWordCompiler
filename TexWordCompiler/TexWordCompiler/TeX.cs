using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TexWordCompiler
{
    public class TeX
    {
        #region enums

        public enum Commands
        {
            emph,
            textit,
            textbf,
            textsc,
        }

        public enum Environments
        {
            list,
            enumeration,
            centre,
            table,
            tabular,
            math,
            Abstract,
        }

        public enum TextType
        {
            section,
            subsection,
            subsubsection,
        }

        public enum Type
        {
            text,
            environment,
            command,
            macro
        }

        public enum ListType
        {
            enumerate,
            itemize,
        }

        #endregion enums

        /// <summary>
        /// stored as macro command, macro output
        /// </summary>
        public static Dictionary<string, string> Macros;

        private static Dictionary<string, string> _E;

        public static Dictionary<string, string> EscapedChars
        {
            get
            {
                if (_E == null)
                {
                    _E = new Dictionary<string, string>();
                    _E.Add("\\\\", "\\");
                    _E.Add("\\%", "%");
                    _E.Add("\\&", "&");
                    _E.Add("\\_", "_");
                    _E.Add("\\^", "^");
                    _E.Add("\\$", "$");
                    _E.Add("\\#", "#");
                    _E.Add("\\{", "{");
                    _E.Add("\\}", "}");
                    _E.Add("\\~", "~");
                }
                return _E;
            }
        }

        public static string RegexLine = @"\\([a-zA-Z]+)(\{(.+)\})?";
        public static string RegexMacroLine = @"\\([a-zA-Z]+)";

        public string Title;
        public string Author;
        public string Date;

        public Dictionary<string, List<string>> DocumentClass;

        public Dictionary<string, List<string>> Packages;

        public string GraphicsPath;

        public Type type;

        public TeX()
        {
        }

        public string Calculate(string thing)
        {
            return "";
        }

        private string StripChars(string input)
        {
            return input.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\\", "");
        }

        public string AddProperty(string property, string optionalParameters, string value)
        {
            string[] ops;
            switch (property)
            {
                case "documentclass":
                    DocumentClass = new Dictionary<string, List<string>>();
                    ops = optionalParameters.Split(',');

                    foreach (string s in ops)
                    {
                        if (!DocumentClass.ContainsKey("optional"))
                        {
                            DocumentClass.Add("optional", new List<string>());
                        }
                        DocumentClass["optional"].Add(StripChars(s));
                    }
                    DocumentClass.Add("type", new List<string>());
                    DocumentClass["type"].Add(StripChars(value));
                    break;

                case "usepackage":
                    if (Packages == null)
                    {
                        Packages = new Dictionary<string, List<string>>();
                    }

                    value = StripChars(value);
                    Packages.Add(value, new List<string>());
                    ops = optionalParameters.Split(',');

                    foreach (string s in ops)
                    {
                        Packages[value].Add(StripChars(s));
                    }
                    break;

                case "graphicspath":
                    GraphicsPath = value;
                    break;

                case "let":
                case "renewcommand":
                    if (Macros == null)
                    {
                        Macros = new Dictionary<string, string>();
                    }
                    Macros.Add(StripChars(value), StripChars(optionalParameters));

                    break;
            }

            return StripChars(value);
        }

        private static string RemoveComments(string line)
        {
            // Temporarily replace an escaped percent with a unit seperator.
            return line.Replace("\\%", ((char)30).ToString()).Split('%')[0].Replace(((char)30).ToString(), "\\%");
        }

        public static List<string> ReadLine(string line)
        {
            string escapedLine = line;

            escapedLine = RemoveComments(escapedLine);

            if (Macros != null)
            {
                foreach (string m in Macros.Keys.Where(x => escapedLine.Contains(x)))
                {
                    escapedLine.Replace(m, Macros[m]);
                }
            }
            List<string> r = new List<string>();

            foreach (string s in EscapedChars.Keys)
            {
                escapedLine = escapedLine.Replace(s, EscapedChars[s]);
            }
            r.Add(escapedLine);

            MatchCollection match = Regex.Matches(escapedLine, RegexMacroLine);

            int i = 1;
            foreach (Match m in match)
            {
                //r[0] = r[0].Replace(m.ToString(), "{" + i++ + "}");
                string result = m.ToString();
                int nothing = escapedLine.IndexOf(result) + result.Length + 1;
                char blah = escapedLine[escapedLine.IndexOf(result) + result.Length + 1];
                if (escapedLine[escapedLine.IndexOf(result) + result.Length] == '{')
                {
                    int braces = 1, counter = escapedLine.IndexOf(result) + result.Length + 1;
                    StringBuilder sb = new StringBuilder();
                    while (braces > 0)
                    {
                        if (escapedLine[counter] == '{' && escapedLine[counter - 1] != '\\')
                        {
                            braces++;
                        }
                        else if (escapedLine[counter] == '}' && escapedLine[counter - 1] != '\\')
                        {
                            braces--;
                        }

                        sb.Append(escapedLine[counter++]);
                    }

                    result += "{" + sb.ToString();
                }

                if (escapedLine.Contains(result))
                {
                    r.Add(result);
                    r[0] = r[0].Replace(result, "{" + i++ + "}");
                }
            }

            for (int a = 1; a < r.Count; a++)
            {
                for (int b = a + 1; b < r.Count; b++)
                {
                    if (r[a].Contains(r[b]))
                    {
                        r[a] = r[a].Replace(r[b], "{" + b + "}");
                    }
                }
            }

            return r;
        }

        public static string ReplaceTeX(string line)
        {
            string text = line;
            //string RegexLine = @"\\([a-zA-Z]+)(\{([a-zA-Z]+)\})?";
            if (Macros != null && Macros.ContainsKey(line))
            {
                text = ReplaceTeX(Macros[line]);
            }
            else if (Regex.IsMatch(text, RegexLine))
            {
                MatchCollection ms = Regex.Matches(text, RegexLine);

                foreach (Match m in ms)
                {
                }
            }

            return "";
        }
    }
}