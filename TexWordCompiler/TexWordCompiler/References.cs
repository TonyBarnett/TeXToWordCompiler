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
        private List<string> _RefTypes;

        private List<string> RefTypes
        {
            get
            {
                if (_RefTypes == null)
                {
                    _RefTypes = new List<string>();

                    _RefTypes.Add("article");
                    _RefTypes.Add("book");
                    _RefTypes.Add("booklet");
                    _RefTypes.Add("conference");
                    _RefTypes.Add("inbook");
                    _RefTypes.Add("incollection");
                    _RefTypes.Add("inproceedings");
                    _RefTypes.Add("manual");
                    _RefTypes.Add("mastersthesis");
                    _RefTypes.Add("misc");
                    _RefTypes.Add("phdthesis");
                    _RefTypes.Add("proceedings");
                    _RefTypes.Add("techreport");
                    _RefTypes.Add("unpublished");
                }
                return _RefTypes;
            }
        }

        private Dictionary<string, Dictionary<FileInfo, int>> _RefFiles;

        public References(FileInfo file)
        {
            _RefFiles = new Dictionary<string, Dictionary<FileInfo, int>>();

            using (StreamReader r = new StreamReader(file.FullName))
            {
                StringBuilder RegexCheck = new StringBuilder();
                foreach (string s in RefTypes)
                {
                    RegexCheck.Append(s + "|");
                }

                string regex = @"\@[" + RegexCheck.ToString().TrimEnd('|') + "]";
                int position = 0;
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    if (Regex.IsMatch(line, regex))
                    {
                        string tag = line.Split('{')[1];

                        if (_RefFiles.ContainsKey(tag))
                        {
                            throw new Exception(string.Format("Trying to add {0} twice, tut tut!", tag));
                        }

                        _RefFiles.Add(tag, new Dictionary<FileInfo, int>());
                        _RefFiles[tag].Add(file, position);
                    }

                    position += line.Length;
                }
            }
        }

        public References(List<FileInfo> files)
        {
            _RefFiles = new Dictionary<string, Dictionary<FileInfo, int>>();
        }

        public string GetAuthorYear(string refKey)
        {
            string author = "";
            int year = 0;
            if (!_RefFiles.ContainsKey(refKey))
            {
                throw new Exception(string.Format("can't find {0} in list of references", refKey));
            }

            FileInfo f = _RefFiles[refKey].Keys.ToList()[0];

            using (StreamReader r = new StreamReader(f.FullName))
            {
                for (int i = 0; i < _RefFiles[refKey][f]; i++)
                {
                    r.Read();
                }

                char current = new char(), previous = new char();
                StringBuilder sb = new StringBuilder();

                // keep reading until you find the first unescaped @ \ie next reference
                while (current != '@' || (current == '@' && previous == '\\'))
                {
                    current = (char)r.Read();
                    if (current != '\\' && previous != '\\')
                    {
                        sb.Append(current);
                    }

                    if (Regex.IsMatch(sb.ToString(), @"author = \{[^\{\}]+\},"))
                    {
                        author = Regex.Match(sb.ToString(), @"author = \{{(?<a>[\{\}]+}\},").Groups["a"].Captures[0].Value;
                    }

                    if (Regex.IsMatch(sb.ToString(), @"year = \{{.+}\},"))
                    {
                        author = Regex.Match(sb.ToString(), @"year = \{{(?<y>[\{\}]+}\},").Groups["y"].Captures[0].Value;
                    }

                    if (author != "" && year != 0)
                    {
                        return string.Format("{0}{1}", author, year);
                    }
                    previous = current;
                }
            }
            throw new Exception("Couldn't find both author and year");
        }
    }
}