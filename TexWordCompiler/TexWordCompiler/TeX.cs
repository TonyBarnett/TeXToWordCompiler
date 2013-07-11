using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        #endregion

        /// <summary>
        /// stored as macro command, macro output
        /// </summary>
        public Dictionary<string, string> Macros;

        private static List<char> _E;
        public static List<char> EscapedChars
        {
            get
            {
                if (_E == null)
                {
                    _E = new List<char>();
                    _E.Add('\\');
                    _E.Add('%');
                    _E.Add('&');
                    _E.Add('_');
                    _E.Add('^');
                    _E.Add('$');
                    _E.Add('#');
                    _E.Add('{');
                    _E.Add('}');
                    _E.Add('~');
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
            return input.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\\","");
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
    }
}