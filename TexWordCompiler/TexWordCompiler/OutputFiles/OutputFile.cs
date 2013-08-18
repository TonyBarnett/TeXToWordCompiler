using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    public class OutputFile : Document
    {
        public List<Document> Contents;

        private StreamWriter _Output;
        private FileInfo _OutFile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namespaces">key = Id, value =NamespaceKey</param>
        /// <param name="file"></param>
        public OutputFile(Dictionary<string, string> namespaces, FileInfo file, string documentName, string thisNamespaceId)
            : base(thisNamespaceId + ":" + documentName)
        {
            _OutFile= file;
            foreach (string key in namespaces.Keys)
            {
                AddAttribute(string.Format(@"xmlns:{0}", key), namespaces[key]);
            }

            Contents = new List<Document>();
        }

        private void CreateDirectory(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                return;
            }
            if (!dir.Parent.Exists)
            {
                CreateDirectory(dir.Parent);
            }
            dir.Create();
        }

        public void Done()
        {
            CreateDirectory(_OutFile.Directory);

            using (_Output = new StreamWriter(_OutFile.FullName))
            {
                _Output.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                foreach (Document d in Contents)
                {
                    Add(d);
                }
                _Output.WriteLine(GetXml());
            }
        }
    }
}