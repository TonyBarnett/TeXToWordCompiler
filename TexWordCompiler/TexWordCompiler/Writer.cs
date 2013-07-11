using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TexWordCompiler
{
    public class Writer
    {
        private DirectoryInfo _Dir;
        private FileInfo _File;

        public Writer(string directory, string fileName)
        {
            _Dir = new DirectoryInfo(directory);
            _File = new FileInfo(string.Format("{0}\\{1}", directory, fileName.Split('.')[0]));

            CreateDirectories();
        }

        private void CreateDirectories()
        {
            DirectoryInfo d = new DirectoryInfo(_Dir.FullName + "/" + _File.Name);
            if (d.Exists)
            {
                d.Delete(true);
            }

            _Dir.CreateSubdirectory(_File.Name);

            _Dir.CreateSubdirectory(string.Format("{0}/_rels", _File.Name));
            _Dir.CreateSubdirectory(string.Format("{0}/customXml", _File.Name));
            _Dir.CreateSubdirectory(string.Format("{0}/docProps", _File.Name));
            _Dir.CreateSubdirectory(string.Format("{0}/word", _File.Name));
        }

        /// <summary>
        /// Appends output to file fileName then add newLine 
        /// character
        /// </summary>
        /// <param name="fileName">relative to input directory</param>
        /// <param name="output"></param>
        public void WriteFile(string fileName, string output)
        {
            FileInfo f = new FileInfo(string.Format("{0}\\{1}", _Dir.FullName, fileName));
            
            using (StreamWriter s = new StreamWriter(f.FullName))
            {
                if (!f.Exists)
                {
                    if (!f.Directory.Exists)
                    {
                        f.Directory.Create();
                    }
                    f.Create();
                }

                s.Write(string.Format("{0}{1}", output, Environment.NewLine));
            }
        }
    }
}