using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace TexWordCompiler
{
    public class Reader : StreamReader
    {
        public enum LineType
        {
            Line,
            Environment,
            ParagraphBreak,

        }

        public class Header
        {
            public enum Type
            {
                Macro,
                section,
                subSection,
                subsubSection,
                line,
                package,
                detail
            }

            /// <summary>
            /// True when the line being Read is "\begin{document}". 
            /// This signals the end of the header information.
            /// </summary>
            public static bool IsEnd = false;

            static string OptionalPattern = @"\[(.+)\]";
            static string nonOptionalPattern = @"\{(.+)\}";

            /// Everything in square brackets that was comma seperated
            /// </summary>
            public List<string> Optionals;
            /// <summary>
            /// everything that was within braces that was comma seperated
            /// </summary>
            public List<string> Values;
            public Dictionary<string, string> Macro;

            public string CommandType = "";

            public Header(string line)
            {
                line = line.Trim();

                if (Regex.IsMatch(line, @"\{\{.+\}\}"))
                {
                    line = line.Replace(@"{{", @"{");
                    line = line.Replace(@"}}", @"}");
                }

                if (line.Length < 1)
                {
                    return;
                }

                else if (line.ToLower() == @"\begin{document}")
                {
                    IsEnd = true;
                    return;
                }

                if (line[0] == '\\') //if it's a command
                {
                    Match m = Regex.Match(line, OptionalPattern); //strip out and save any optional params
                    if (m.Success)
                    {
                        Optionals = new List<string>();

                        Optionals.AddRange(m.Groups[1].ToString().Split(','));
                    }

                    m = Regex.Match(line, nonOptionalPattern); //strip out and save non-optional params

                    if (m.Success)
                    {
                        Match subM = Regex.Match(line, @"\{(.+)\}\{(.+)\}"); //if it's a renewcommand then save it as a macro
                        if (subM.Success)
                        {
                            Macro = new Dictionary<string, string>();

                            Macro.Add(subM.Groups[1].ToString(), subM.Groups[2].ToString());

                        }
                        else
                        {
                            Values = new List<string>();
                            Values.AddRange(m.Groups[1].ToString().Split(','));
                        }
                    }

                    if (Optionals == null && Values == null)
                    {
                        string[] s = line.Substring(1).Split('\\'); //Ignore first \

                        if (Macro != null) //\renewcommand{}{}
                        {
                            CommandType = s[0].Split('{')[0];
                        }
                        else if (s[0] == "let")//e.g. \let\section\stdsection
                        {
                            CommandType = s[0];
                            Macro = new Dictionary<string, string>();
                            Macro.Add(s[1], s[2]);
                        }
                        else//e.g. \clubpenalty=3000
                        {
                            CommandType = line.Substring(0, line.IndexOf('='));
                            Values = new List<string>();
                            Values.Add(line.Substring(line.IndexOf('=') + 1).Split('%')[0]);
                        }
                    }

                    else
                    {
                        if (Optionals != null)
                        {
                            CommandType = line.Substring(1, line.IndexOf('[') - 1);
                        }
                        else
                        {
                            CommandType = line.Substring(1, line.IndexOf('{') - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used to work out Wordified form of a LaTeX line
        /// 
        /// </summary>
        public class Line
        {
            private string _Pattern1 = @"\\(.+)\{(.+)\}\{(.+)\}[(.+)]"; //similar to table \thing{}{}[]
            private string _Pattern2 = @"\\(.+)\{(.+)\}\{(.+)\}"; //similar to renewcommand \thing{}{}
            private string _Pattern3 = @"\\(.+)\{(.+)\}"; //single command \thing{}
            //private string pattern4 = @"\\(.+)\{(.+)"; //multiLine command \thing{\n}
            private string _Pattern5 = @"\\.+[^\\]\{.+\\.+[^\\]\{"; //single block with embedded \thing1{\thing2{blah}}

            public bool EndParagraph = false;
            /// <summary>
            /// List all words that are in italics,
            /// 0 based index.
            /// </summary>
            public List<int> Italics;

            /// <summary>
            /// List all words that are in bold,
            /// 0 based index.
            /// </summary>
            public List<int> Bold;

            /// <summary>
            /// Line in plain english
            /// </summary>
            public string ThisLine;

            /// <summary>
            /// store each bullet point/ numbered list element.
            /// </summary>
            public List<string> listEnvironment;

            //public TeX.ListType ListType;

            Dictionary<int, int> toFrom = new Dictionary<int, int>();

            public List<string> Output = new List<string>();

            public List<string> Type = new List<string>();
            public List<string> Params = new List<string>();

            public List<string> Optionals = new List<string>();
            /// <summary>
            /// to do much later... work out how Word stores 
            /// equations or just work it out as text, for now 
            /// lets store as-is and ignore.
            /// </summary>
            public string Equation;

            public void ParseLine(string line)
            {
                //use braces to know if you're within a brace or not and then 
                //assign to the correct list accordingly

                // Gotta remove the escaped ones to only count the real ones.
                string temp = line.Replace("\\{", "").Replace("\\}", "").Replace("\\\\", "");

                if (temp == Environment.NewLine)
                {
                    EndParagraph = true;
                    return;
                }

                //int braces = 0; // keep a record of how many open braces we've seen
                int listCounter = 0;

                List<StringBuilder> sb = new List<StringBuilder>();
                sb.Add(new StringBuilder()); // use a list 

                for (int i = 0; i < temp.Length; i++) //for each character in escaped string
                {
                    switch (temp[i])
                    {
                        case '\\':
                            StringBuilder miniSb = new StringBuilder();
                            i++;
                            while (i < temp.Length && temp[i] != ' ' && temp[i] != '{' )
                            {
                                miniSb.Append(temp[i++]);
                            }

                            if (i < temp.Length && temp[i] == ' ')
                            {
                                sb[listCounter].Append(miniSb.ToString());
                            }
                            else
                            {
                                Type.Add(miniSb.ToString());
                            }

                            i--;// we're at a { or a space but when the loop counter increments 
                            break; //we'll skip over it without this

                        case '{':
                            sb.Add(new StringBuilder());
                            toFrom.Add(sb.Count - 1, listCounter);
                            sb[listCounter].Append('{').AppendFormat(@"{0}", sb.Count - 1).Append('}');
                            listCounter = sb.Count - 1;
                            break;
                        case '}':
                            listCounter = toFrom[listCounter];
                            break;

                        default:
                            sb[listCounter].Append(temp[i]);
                            break;
                    }
                }

                foreach (StringBuilder s in sb)
                {
                    Output.Add(s.ToString());
                }


                //string thing = line;
                //List<string> type = new List<string>();
                //List<string> output = new List<string>();
                //for (int i = 0; i < line.Replace("\\{","").Count(t => t == '{'); i++)
                //{
                //    thing = thing.Substring(thing.Replace("\\{","").Replace("\\\\","").IndexOf('\\') + 1, thing.Replace("\\}","").LastIndexOf('}') - thing.IndexOf('\\') - 1);
                //    type.Add(thing.Substring(0,thing.Replace("\\{","").IndexOf('{')));
                //    output.Add(thing.Substring(thing.Replace("\\{","").IndexOf('{')+1));
                //}
            }

            public void GetNextBlock(StreamReader r)
            {
                //Read line, work out what it says
                MatchCollection match;

                string line = r.ReadLine();

                line = line.TrimStart(new char[] { '\t' });


                if (line.Replace(@"\%", "").Contains('%'))// remove any comments
                {
                    line = line.Replace(@"\%", "").Split('%')[0];
                }

                if (line.Count() == 0)//possible if line was a comment or just spacing
                {
                    return;
                }

                //if there are more unescapes left braces than unescaped right braces then multi-line so 
                // consume lines until you find the end.
                while (line.Replace("\\{", "").Count(t => t == '{') > line.Replace("\\}", "").Count(t => t == '}'))
                {
                    line += r.ReadLine();
                }

                while (line[0] == '%')
                {
                    line = r.ReadLine();
                }

                if (Regex.IsMatch(line, _Pattern5))// \blah{ anything \blah2{} gst }
                {
                    ParseLine(line);
                }

                else if (Regex.IsMatch(line, _Pattern1))// \blah{}{}[]
                {
                    match = Regex.Matches(line, _Pattern1);

                    foreach (Match m in match)
                    {
                        Type.Add(m.Groups[1].Value);
                        Output.Add(m.Groups[2].Value);
                        Params.Add(m.Groups[3].Value);
                        Optionals.Add(m.Groups[4].Value);
                    }
                }

                else if (Regex.IsMatch(line, _Pattern2))// \blah{}{}
                {
                    match = Regex.Matches(line, _Pattern2);

                    foreach (Match m in match)
                    {
                        Type.Add(m.Groups[1].Value);
                        Output.Add(m.Groups[2].Value);
                        Params.Add(m.Groups[3].Value);
                    }
                }

                else if (Regex.IsMatch(line, _Pattern3))// \blah{}
                {
                    match = Regex.Matches(line, _Pattern3);

                    foreach (Match m in match)
                    {
                        Type.Add(m.Groups[1].Value);
                        Output.Add(m.Groups[2].Value);
                    }
                }
            }

            /// <summary>
            /// Basic constructor. Nothing to really initialse
            /// </summary>
            public Line()
            {

            }
        }

        public event SetLabel Sl;
        public int FileLength;

        private TeX _Tex;

        public Reader(string fileName)
            : base(fileName)
        {
            _Tex = new TeX();

            //Sl(string.Format("file length = {0}", FileLength.ToString("#,#0")));
        }


        /// <summary>
        /// returns either a word, environment, symbol etc.
        /// </summary>
        /// <returns></returns>
        public Header GetHeaderInformation()
        {
            StringBuilder line = new StringBuilder();

            if (Peek() == '%') // Consume line and newline before processing
            {                  // i.e. ignore the line, it is a comment
                while (Peek() != '\n' && Peek() != '\r')
                {
                    Read();
                }

                while (Peek() == '\n' || Peek() == '\r') //Consume and ignore new line characters
                {
                    Read();
                }
            }

            while (Peek() != '\n' && Peek() != '\r')
            {
                line.Append((char)Read());

            }

            while (Peek() == '\n' || Peek() == '\r') //Consume and ignore new line characters
            {
                Read();
            }

            Header h = new Header(line.ToString());

            if (h.CommandType == "") // was either a begin document or something I didn't understand 
                // so I feel I should ignore :)
                return null;

            return h;
        }

        public Line ParseLine()
        {
            Line l = new Line();
            l.GetNextBlock(this);
            return l;
        }

    }
}