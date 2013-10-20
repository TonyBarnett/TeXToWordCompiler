using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TexWordCompiler
{
    public class Writer
    {
        /// <summary>
        /// Directory to write new file.
        /// </summary>
        private DirectoryInfo _Dir;
        
        /// <summary>
        /// Name of new file.
        /// </summary>
        private FileInfo _File;

        private TextReader _Reader;
        private OutputFiles.Output _Output;

        public Writer(string directory, string fileName)
        {
            _Dir = new DirectoryInfo(directory);
            _File = new FileInfo(string.Format("{0}\\{1}", directory, fileName.Split('.')[0]));
            _Output = new OutputFiles.Output();
            _Reader = new Reader(_File.FullName);
        }

        public void NewParagraph()
        {
            _Output.NewParagragh();
        }

        public void WriteLine(string line)
        {
            _Output.AddLine(line);
        }

        public void Done()
        {
            _Output.Save(_Dir.FullName);
        }

        public void Read()
        {
            Reader.LineType lineType;
            using (Reader r = new Reader(_File.FullName))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ParseLine();
                    Reader.Header header = r.GetHeaderInformation();
                }
            }
        }
    }
}