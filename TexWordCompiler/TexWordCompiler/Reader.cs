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
                        Match subM = Regex.Match(line,  @"\{(.+)\}\{(.+)\}"); //if it's a renewcommand then save it as a macro
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

            public TeX.ListType Type;

            Dictionary<int, int> toFrom = new Dictionary<int, int>();

            /// <summary>
            /// to do much later... work out how Word stores 
            /// equations or just work it out as text, for now 
            /// lets store as-is and ignore.
            /// </summary>
            public string Equation;

            //public void GetNextBlock(StreamReader r)
            //{
            //    //Maybe read entire line instead then Regex through it to get information>
            //    //will probably be quicker and definately be easier?

            //    //consume all new line characters
            //    while ((char)r.Peek() == '\n' || (char)r.Peek() == '\r')
            //    {
            //        r.Read();
            //    }
            //    //place current character into 'current' then
            //    //work out what to do and append to sb.
            //    //When done chuck sb out to Line
            //    StringBuilder sb = new StringBuilder();
            //    char current = new char();
            //    string specialWord = "";
            //    int word = 0; //current word number
            //    //if not end of line or begin of comment
            //    while ((char)r.Peek() != '\n' && (char)r.Peek() != '\r' && (char)r.Peek() != '%')
            //    {
            //        current = (char)r.Read();

            //        switch (current)
            //        {
            //            case '\\':
            //                if (TeX.EscapedChars.Contains((char)r.Peek()))
            //                {
            //                    sb.Append((char)r.Read());
            //                    break;
            //                }

            //                StringBuilder s = new StringBuilder();
            //                while ((char)r.Peek() != '\n' &&
            //                    (char)r.Peek() != '\r' &&
            //                    (char)r.Peek() != '{' &&
            //                    (char)r.Peek() != ' ')
            //                {
            //                    s.Append((char)r.Read());
            //                }

            //                if ((char)r.Peek() == ' ' || //if a macro (either built in 
            //                    (char)r.Peek() == '\n' || // or custom
            //                    (char)r.Peek() == '\r'
            //                    )
            //                {

            //                    ThisLine = specialWord = s.ToString();
            //                }

            //                else if ((char)r.Peek() == '{') //if a command or environment
            //                {
            //                    bool done = false;
            //                    while (!done)
            //                    {
            //                        current = (char)r.Read();
            //                        while (current != '}')
            //                        {
            //                            current = (char)r.Read(); //consume all new lines and tabs
            //                            while (current == '\n' || current == '\r' || current == '\t')
            //                            {
            //                                current = (char)r.Read();
            //                            }

            //                            sb.Append(current);
            //                            current = (char)r.Peek();
            //                        }
            //                        current = (char)r.Read(); //consume close brace
            //                        if ((char)r.Peek() != '{')
            //                        {
            //                            done = true;
            //                        }
            //                    }
            //                }

            //                else
            //                    throw new Exception(string.Format("{0} was an unexpected character", (char)r.Read()));

            //                //if { then do something else work out what you 
            //                //just read
            //                break;

            //            case ' ':
            //                word++;
            //                sb.Append(current);
            //                break;

            //            default:
            //                sb.Append(current);
            //                break;
            //        }

            //    }

            //        switch (sb.ToString()) // Work out what to do with what you've read
            //        {
            //            case "enumerate":
            //            case "itemize":
            //                string line = "";
            //                string block = "";
            //                while (line != string.Format("\\end{{{0}}}",sb.ToString()))
            //                {
            //                    line = line.Replace("\t", "").Replace("\r", "").Replace("\n", "");
            //                    block += line;
            //                    line = r.ReadLine();
            //                }
            //                listEnvironment = new List<string>();
            //                listEnvironment.AddRange(block.Substring(5).Replace("\\item ","|").Split('|'));
            //                break;
            //            case "section":
            //            case "section*":

            //                    break;
            //            case "subsection":
            //            case "subsection*":

            //                    break;
            //            case "subsubsection":
            //            case "subsubsection*":

            //                    break;

            //            default:
            //                break;
            //        }

            //    current = (char)r.Read();

            //    if (current == '%')  //if a comment is added to the 
            //    {                   //line consume the rest of the 
            //        while (r.Peek() != '\n') // line and ignore
            //        {
            //            r.Read();
            //        }
            //    }

            //    //while (current == '\n' ||
            //    //    current == '\r')
            //    //{
            //    //    current = (char)r.Read();
            //    //}
            //}

            public void ParseLine(string line)
            {
                //use braces to know if you're within a brace or not and then 
                //assign to the correct list accordingly

                List<string> type = new List<string>();
                List<string> output = new List<string>();

                // Gotta remove the escaped ones to only count the real ones.
                string temp = line.Replace("\\{", "").Replace("\\}", "").Replace("\\\\", "");

                int braces = 0; // keep a record of how many open braces we've seen
                int listCounter = 0;

                List<StringBuilder> sb = new List<StringBuilder>();
                sb.Add(new StringBuilder()); // use a list 

                for (int i = 0; i < temp.Length; i++) //for each character in escaped string
                {
                    switch (temp[i])
                    {
                        case '\\':
                            StringBuilder miniSb = new StringBuilder();
                            while (temp[i] != ' ' && temp[i] != '{' && i < temp.Length)
                            {
                                miniSb.Append(temp[i++]);
                            }

                            if (temp[i] == ' ')
                            {
                                sb[braces].Append(miniSb.ToString());
                            }
                            else
                            {
                                type.Add(miniSb.ToString());
                            }

                            i--;// we're at a { or a space but when the loop counter increments 
                            break; //we'll skip over it without this

                        case '{':
                            //sb[braces++].Append('{').AppendFormat(@"{0}", ++listCounter).Append('}');
                            sb[braces].Append('{').AppendFormat(@"{0}", listCounter).Append('}');
                            toFrom.Add(++listCounter, braces++);
                            sb.Add(new StringBuilder());
                            break;
                        case '}':

                            braces--;
                            break;

                        default:
                            sb[listCounter].Append(temp[i]);
                            break;
                    }
                }

                foreach (StringBuilder s in sb)
                {
                    output.Add(s.ToString());
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
                string pattern1 = @"\\(.+)\{(.+)\}\{(.+)\}[(.+)]"; //similar to table \thing{}{}[]
                string pattern2 = @"\\(.+)\{(.+)\}\{(.+)\}"; //similar to renewcommand \thing{}{}
                string pattern3 = @"\\(.+)\{(.+)\}"; //single command \thing{}
                string pattern4 = @"\\(.+)\{(.+)"; //multiLine command \thing{\n}
                string pattern5 = @"\\.+[^\\]\{.+\\.+[^\\]\{"; //single block with embedded \thing1{\thing2{blah}}

                MatchCollection match;

                string thing = r.ReadLine();

                List<string> type = new List<string>();
                List<string> output = new List<string>();
                List<string> parameters = new List<string>();
                List<string> optionalParameters = new List<string>();

                thing = thing.TrimStart(new char[] { '\t' });

                if (thing.Count() == 0 || thing[0] == '%')
                {
                    return;
                }

                //if there are more unescapes left braces than unescaped right braces then multi-line so 
                // consume lines until you find the end.
                while (thing.Replace("\\{", "").Count(t => t == '{') > thing.Replace("\\}", "").Count(t => t == '}'))
                {
                    thing += r.ReadLine();
                }

                while (thing[0] == '%')
                {
                    thing = r.ReadLine();
                }

                if (Regex.IsMatch(thing, pattern5))// \blah{ anything \blah2{}
                {
                    ParseLine(thing);
                }

                else if (Regex.IsMatch(thing, pattern1))// \blah{}{}[]
                {

                    match = Regex.Matches(thing, pattern1);

                    foreach (Match m in match)
                    {
                        type.Add(m.Groups[1].Value);
                        output.Add(m.Groups[2].Value);
                        parameters.Add(m.Groups[3].Value);
                        optionalParameters.Add(m.Groups[4].Value);
                    }
                }

                else if (Regex.IsMatch(thing, pattern2))// \blah{}{}
                {
                    match = Regex.Matches(thing, pattern2);

                    foreach (Match m in match)
                    {
                        type.Add(m.Groups[1].Value);
                        output.Add(m.Groups[2].Value);
                        parameters.Add(m.Groups[3].Value);
                    }
                }

                else if (Regex.IsMatch(thing, pattern3))// \blah{}
                {
                    match = Regex.Matches(thing, pattern3);

                    foreach (Match m in match)
                    {
                        type.Add(m.Groups[1].Value);
                        output.Add(m.Groups[2].Value);
                    }
                }
            }

            /// <summary>
            /// Basic constructor. Nothing to really initialse
            /// </summary>
            /// <param name="r"></param>
            public Line()
            {
 
            }

            //public Line(StreamReader r)
            //{
            //    string pattern1 = @"\\(.+)\{(.+)\}\{(.+)\}[(.+)]"; //similar to table \thing{}{}[]
            //    string pattern2 = @"\\(.+)\{(.+)\}\{(.+)\}"; //similar to renewcommand \thing{}{}
            //    string pattern3 = @"\\(.+)\{(.+)\}"; //single command \thing{}
            //    string pattern4 = @"\\(.+)\{(.+)"; //multiLine command \thing{\n}

            //    Match m;

            //    string thing = r.ReadLine();

            //    while (thing[0] == '%')
            //    {
            //        thing = r.ReadLine();
            //    }
            //    while (Regex.IsMatch(thing, pattern4) && //If thing runs over multiple lines, grab them all
            //        !Regex.IsMatch(thing, pattern3))
            //    {
            //        thing += r.ReadLine();
            //    }

            //    if (Regex.IsMatch(thing, pattern1))// \blah{}{}[]
            //    {

            //    }

            //    else if (Regex.IsMatch(thing, pattern2))// \blah{}{}
            //    {

            //    }

            //    else if (Regex.IsMatch(thing, pattern3))// \blah{}
            //    {
            //        m = Regex.Match(thing, pattern3);

            //        string type = m.Groups[1].Value;
            //        string output = m.Groups[2].Value;
            //    }

            //}
        }

        public event SetLabel Sl;
        public int FileLength;

        private TeX _Tex;

        public Reader(string fileName)
            : base(fileName)
        {
            Thread t = new Thread(new ThreadStart(GetFileLength));
            t.Start();
        }

        private void GetFileLength()
        {
            FileLength = (int)BaseStream.Length;

            Sl(string.Format("file length = {0}", FileLength.ToString("#,#0")));

            _Tex = new TeX();
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

            Header l = new Header(line.ToString());

            if (l.CommandType == "") // was either a begin document or something I didn't understand 
                                       // so I feel I should ignore :)
                return null;

            return l;


        }

        public string ParseLine()
        {
            Line l = new Line();
            l.GetNextBlock(this);
            return l.ThisLine;
        }

    }
}