using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TexWordCompiler
{
    public class Reader : StreamReader
    {
        private TeX _Tex;

        /// <summary>
        /// Strip any escaped special characters from a string. Useful if you want
        /// to count true open and close braces for instance.
        /// </summary>
        /// <param name="line">a TeX line.</param>
        /// <returns>a nonTeX line.</returns>
        private string EscapelessLine(string line)
        {
            return line.Replace("\\\\", "").Replace("\\{", "").Replace("\\}", "").Replace("\\[", "").Replace("\\]", "");
        }

        public Reader(string fileName)
            : base(fileName)
        {
            _Tex = new TeX();
        }

        public void ParseHeader(Stream s)
        {
            #region Regexs

            // \thing{blah}
            string regexPattern = @"\\\{w+}\{{\w+}\}";
            //\thing[optionalBlah]{blah}
            string regexPatternOptionals = @"\\\{w+}\[{\w+}\]\{{\w+}\}";
            //\thing{blah1}{blah2}
            string regexPatternMacro = @"\\\{w+}\{{\w+}\}\{{\w+}\}";
            //\thing
            string regexPatternNoParams = @"\\{\w+}";

            #endregion Regexs

            bool done = false; // keep reading header information until I say so!

            // Let us begin...
            using (StreamReader r = new StreamReader(s))
            {
                StringBuilder sb = new StringBuilder();

                while (!done && !r.EndOfStream)
                {
                    string line = r.ReadLine();

                    // We should ignore the line if it's empty and move on.
                    while (line == Environment.NewLine)
                    {
                        line = r.ReadLine();
                    }

                    // We need to make sure we have the whole statement.
                    while (EscapelessLine(line).Split('{').Length + EscapelessLine(line).Split('[').Length !=
                        EscapelessLine(line).Split('}').Length + EscapelessLine(line).Split(']').Length)
                    {
                        line += r.ReadLine();
                    }

                    // Let's remove any newLine characters.
                    line.Replace("\n", "").Replace("\r", "");

                    while (line[0] == '%')
                    {
                        line = ReadLine();
                    }

                    if (line == @"\\begin{document}")
                    {
                        // We're finished, move onto the next line and let's get outta here.
                        done = true;
                        r.ReadLine();
                        break;
                    }

                    else if (Regex.IsMatch(line, regexPatternMacro))
                    {
                    }

                    else if (Regex.IsMatch(line, regexPatternOptionals))
                    {
                    }

                    else if (Regex.IsMatch(line, regexPattern))
                    {
                    }

                    else if (Regex.IsMatch(line, regexPatternNoParams))
                    {
                    }

                    else
                    {
                        throw new Exception(string.Format("I have no idea what \"{0}\" is in terms of header information.", line));
                    }
                    if (r.EndOfStream)
                    {
                        throw new Exception("Got through the whole file without a \\begin{Document}, This means something went wrong or your input file wasn't right!");
                    }
                }
            }
        }

        public List<string> ParseLine()
        {
            List<string> output = new List<string>();
            StringBuilder sb = new StringBuilder();
            using (StreamReader r = this)
            {
                sb.Append(r.ReadLine());
                while (CountChar(sb.ToString(), '{') != CountChar(sb.ToString(), '{'))
                {
                    sb.Append(r.ReadLine());
                }
            }

            int j = 1;

            //MatchCollection mc = Regex.Matches(output[0], TeX.RegexLine);

            //foreach (Match m in mc)
            //{
            //    output[0] = output[0].Replace(m.ToString(), "{" + j++ + "}");
            //    output.Add(m.ToString());
            //}
            return TeX.ReadLine(sb.ToString());
        }

        private int CountChar(string line, char c, char escapeChar = '\\')
        {
            string t = line.Replace(escapeChar.ToString() + c.ToString(), "");

            return t.Count(x => x == c);
        }
    }
}