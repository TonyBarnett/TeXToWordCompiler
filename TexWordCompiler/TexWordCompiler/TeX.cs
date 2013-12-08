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
            string regexLine = @"\\([a-zA-Z]+)(\{([a-zA-Z]+)\})?";
            string escapedLine = line;

            escapedLine = RemoveComments(escapedLine);

            if (Macros != null)
            {
                foreach (string m in Macros.Keys)
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

            MatchCollection match = Regex.Matches(escapedLine, regexLine);

            int i = 1;
            foreach (Match m in match)
            {
                r[0] = r[0].Replace(m.ToString(), "{" + i++ + "}");
                r.Add("\\" + m.ToString()); // The compiler will interpret \thing as {tab}hing if the first \ is not escaped
            }

            return r;
        }

        public static string ReplaceTeX(string line)
        {
            string text = line;
            string regexPattern = @"\\([a-zA-Z]+)(\{([a-zA-Z]+)\})?";
            if (Macros != null && Macros.ContainsKey(line))
            {
                text = ReplaceTeX(Macros[line]);
            }
            else if (Regex.IsMatch(text, regexPattern))
            {
                MatchCollection ms = Regex.Matches(text, regexPattern);

                foreach (Match m in ms)
                {
                }
            }

            return "";
        }
    }
}