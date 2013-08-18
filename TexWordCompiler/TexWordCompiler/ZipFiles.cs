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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">directory to put output file</param>
        public ZipFiles(DirectoryInfo dir)
        {
            _Dir = dir;
        }


        public void Zip(List<DirectoryInfo> dirs, List<FileInfo> files)
        {
            SevenZipCompressor.SetLibraryPath(@"C:\\Program Files\\7-Zip\\7z.dll");

            using (Stream output = new FileStream(_Dir.FullName + "\\test.docx", FileMode.OpenOrCreate))
            {
                using (Stream input = new MemoryStream())
                {
                    foreach (DirectoryInfo dir in dirs)
                    {
                        using (Stream s = new MemoryStream())
                        {
                            for (int i = 0; i < s.Length; i++)
                            {
                                input.WriteByte((byte)s.ReadByte());
                            }
                        }
                    }

                    foreach (FileInfo f in files)
                    {
                        using (Stream s = new FileStream(f.FullName, FileMode.Open))
                        {
                            for (int i = 0; i < s.Length; i++)
                            {
                                input.WriteByte((byte)s.ReadByte());
                            }
                        }
                    }

                    SevenZipCompressor c = new SevenZipCompressor();
                    c.CompressionMethod = CompressionMethod.Copy;
                    c.ArchiveFormat = OutArchiveFormat.Zip;

                    

                    c.CompressStream(input, output);

                }
            }
        }
    }
}