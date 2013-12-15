using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TexWordCompiler
{
    internal class References
    {
        public enum RefType
        {
            article,
            book,
            booklet,
            conference,
            inbook,
            incollection,
            inproceedings,
            manual,
            mastersthesis,
            misc,
            phdthesis,
            proceedings,
            techreport,
            unpublished,
        }

        private Dictionary<RefType, List<string>> RefStyle;

        private Dictionary<string, Dictionary<FileInfo, int>> _RefFiles;

        public References(FileInfo file)
        {
            // The plan is: read through the file, find alll the @s,
            // then store the key (@notThis{This,), the file name, and the position where we found the @
            _RefFiles = new Dictionary<string, Dictionary<FileInfo, int>>();
            RefStyle = new Dictionary<RefType, List<string>>();

            using (StreamReader r = new StreamReader(file.FullName))
            {
                int position = 0;
                while (!r.EndOfStream)
                {
                    char c = (char)r.Read();
                    position++;
                    if (c == '@')
                    {
                        int otherPosition = 0;
                        while (c != '{')
                        {
                            c = (char)r.Read();
                            otherPosition++;
                        }
                        otherPosition++;

                        StringBuilder sb = new StringBuilder();
                        while (c != ',')
                        {
                            c = (char)r.Read();
                            if (c != ',')
                            {
                                sb.Append(c);
                                otherPosition++;
                            }
                        }

                        _RefFiles.Add(sb.ToString(), new Dictionary<FileInfo, int>());

                        _RefFiles[sb.ToString()].Add(file, position - 1);

                        position += otherPosition;
                    }
                }
            }
        }

        public References(List<FileInfo> files)
        {
            _RefFiles = new Dictionary<string, Dictionary<FileInfo, int>>();
            RefStyle = new Dictionary<RefType, List<string>>();
        }

        /// <summary>
        /// Defines how the reference is printed and the order.
        /// </summary>
        /// <param name="refType"></param>
        /// <param name="parts"></param>
        public void AddRefStylePart(RefType refType, List<string> parts)
        {
            RefStyle.Add(refType, new List<string>());
            foreach (string s in parts)
            {
                RefStyle[refType].Add(s);
            }
        }

        /// <summary>
        /// Gets the authors and year of reference who's key is refKey
        /// </summary>
        /// <param name="refKey"></param>
        /// <returns></returns>
        public string GetAuthorYear(string refKey)
        {
            int year = 0;
            if (!_RefFiles.ContainsKey(refKey))
            {
                throw new Exception(string.Format("can't find {0} in list of references", refKey));
            }

            FileInfo f = _RefFiles[refKey].Keys.ToList()[0];

            //count the number of open braces - closed braces
            // We start counting after we've passed the first brace so start counting at 1

            string reference = GetRefText(refKey);

            string author = GetRefPart(reference, "author");

            if (!int.TryParse(GetRefPart(reference, "year"), out year))
            {
                throw new Exception(string.Format("cant make an integer out of {0}", GetRefPart(reference, "author")));
            }

            return string.Format("{0}, {1}", author, year);

            throw new Exception("Couldn't find both author and year");
        }

        public string GetReference(string refKey)
        {
            RefType type;
            string reference = GetRefText(refKey, out type);
            StringBuilder sb = new StringBuilder();
            if (!RefStyle.ContainsKey(type))
            {
                throw new Exception("invalid type " + type.ToString());
            }

            foreach (string s in RefStyle[type])
            {
                sb.Append(GetRefPart(reference, s));
                sb.Append(", ");
            }
            if (sb.Length == 0)
            {
                throw new Exception("Something went wrong. Quick, blame a programmer!");
            }
            return sb.ToString().Substring(0, sb.Length - 2);
        }

        private string GetRefText(string refKey)
        {
            if (!_RefFiles.ContainsKey(refKey))
            {
                throw new Exception(string.Format("can't find {0} in list of references", refKey));
            }

            FileInfo f = _RefFiles[refKey].Keys.ToList()[0];

            StringBuilder sb = new StringBuilder();
            using (StreamReader r = new StreamReader(f.FullName))
            {
                // Read until refernce in file.
                for (int i = 0; i < _RefFiles[refKey][f]; i++)
                {
                    r.Read();
                }

                char c = new char();

                while (c != '{')
                {
                    if (r.EndOfStream)
                    {
                        throw new Exception("bad BibTeX file");
                    }
                    sb.Append(c = (char)r.Read());
                }

                sb.Append((char)r.Read()); // Skip the open brace.

                int openBrace = 1;

                while (openBrace != 0)
                {
                    if (r.EndOfStream)
                    {
                        throw new Exception("bad BibTeX file");
                    }

                    c = (char)r.Read();

                    if (c == '{')
                    {
                        openBrace++;
                    }
                    else if (c == '}')
                    {
                        openBrace--;
                    }

                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private string GetRefText(string refKey, out RefType type)
        {
            string text = GetRefText(refKey);

            string t = Regex.Match(text, @"@(?<t>[^\{\}]+)\{").Groups["t"].Captures[0].Value;

            type = (RefType)Enum.Parse(typeof(RefType), t.ToString());

            return text;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="referenceString">A bibTeX reference</param>
        /// <returns></returns>
        private string GetRefPart(string referenceString, string part)
        {
            StringBuilder sb = new StringBuilder();

            referenceString = referenceString.Substring(referenceString.IndexOf(part) + part.Length);
            referenceString = referenceString.Substring(referenceString.IndexOf('{') + 1);

            int braces = 1, i = 0;

            while (braces != 0)
            {
                if (referenceString[i] == '{')
                {
                    braces++;
                }
                if (referenceString[i] == '}')
                {
                    braces--;
                }
                if (braces != 0)
                {
                    sb.Append(referenceString[i++]);
                }
            }

            return sb.ToString();
        }
    }
}