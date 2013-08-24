using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenZip;
using System.IO;

namespace TexWordCompiler
{
    class ZipFiles
    {
        private DirectoryInfo _Dir;

        private Dictionary<string, string> _Directories;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">directory to put output file</param>
        public ZipFiles(DirectoryInfo dir)
        {
            _Dir = dir;

            if (!_Dir.Exists)
            {
                _Dir.Create();
            }

            _Directories = new Dictionary<string, string>();
        }

        public void AddFolder(DirectoryInfo dir, DirectoryInfo root)
        {
            //Add all files in this folder...
            foreach (FileInfo f in dir.GetFiles())
            {
                FileInfo outFileFull =  new FileInfo(f.FullName.Replace(dir.Parent.FullName, _Dir.FullName));
                string outFile = f.FullName.Replace(dir.Parent.FullName + "\\", "");

                _Directories.Add(outFile, f.FullName);
            }
            //... then go through all child directories and do the same
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                AddFolder(d, root);
            }
        }

        public void AddFolder(DirectoryInfo dir)
        {
            //Add all files in this folder...
            foreach (FileInfo f in dir.GetFiles())
            {
                FileInfo outFileFull = new FileInfo(f.FullName.Replace(dir.Parent.FullName, _Dir.FullName));
                string outFile = f.FullName.Replace(dir.Parent.FullName + "\\", "");

                _Directories.Add(outFile, f.FullName);
            }

            //... then go through all child directories and do the same
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                AddFolder(d, dir);
            }
        }

        /// <summary>
        /// Only to be used for files in the root directory, otherwise 
        /// the directory structure will be lost
        /// </summary>
        /// <param name="file"></param>
        public void AddFile(FileInfo file)
        {
            _Directories.Add( file.Name, file.FullName);
        }

        public void Zip()
        {
            if (_Directories != null)
            {
                SevenZipCompressor.SetLibraryPath(@"C:\\Program Files\\7-Zip\\7z.dll");
                SevenZipCompressor c = new SevenZipCompressor();
                c.DirectoryStructure = true;
                c.PreserveDirectoryRoot = false;
                c.CompressionMode = CompressionMode.Create;
                c.ArchiveFormat = OutArchiveFormat.Zip;
                c.PreserveDirectoryRoot = true;


                c.CompressFileDictionary(_Directories, _Dir.FullName + "\\output.docx");
            }
        }
    }
}