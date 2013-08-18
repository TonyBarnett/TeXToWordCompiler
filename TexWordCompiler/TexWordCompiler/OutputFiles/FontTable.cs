using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    class FontTable
    {
        public FontTable(DirectoryInfo doc)
        {
            Dictionary<string,string> namespaces = new Dictionary<string,string>();
            namespaces.Add("r","http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            namespaces.Add("w","http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            OutputFile f = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\word\\fontTable.xml"), "fonts","w");

            #region calibri
            Document font = new Document("w:font", "w:name", "Calibri");

            font.Add(new Document("w.panose1", "w:val", "020F0502020204030204"));
            font.Add(new Document("w.charset", "w:val", "00"));
            font.Add(new Document("w.family", "w:val", "swiss"));
            font.Add(new Document("w.pitch", "w:val", "variable"));

            Dictionary<string, string> ns = new Dictionary<string, string>();

            ns.Add("w:usb0", "E00002FF");
            ns.Add("w:usb1", "4000ACFF");
            ns.Add("w:usb2", "00000001");
            ns.Add("w:usb3", "00000000");
            ns.Add("w:csb0", "0000019F");
            ns.Add("w:csb1", "00000000");

            font.Add(new Document("w:sig", ns));
            #endregion

            f.Add(font);

            #region Times new roman
            font = new Document("w:font", "w:name", "Times New Roman");

            font.Add(new Document("w.panose1", "w:val", "02020603050405020304"));
            font.Add(new Document("w.charset", "w:val", "00"));
            font.Add(new Document("w.family", "w:val", "roman"));
            font.Add(new Document("w.pitch", "w:val", "variable"));

            ns = new Dictionary<string, string>();

            ns.Add("w:usb0", "E0002AFF");
            ns.Add("w:usb1", "C0007843");
            ns.Add("w:usb2", "00000009");
            ns.Add("w:usb3", "00000000");
            ns.Add("w:csb0", "000001FF");
            ns.Add("w:csb1", "00000000");

            font.Add(new Document("w:sig", ns));
            #endregion

            f.Add(font);

            #region Cambria
            font = new Document("w:font", "w:name", "Cambria");

            font.Add(new Document("w.panose1", "w:val", "02040503050406030204"));
            font.Add(new Document("w.charset", "w:val", "00"));
            font.Add(new Document("w.family", "w:val", "roman"));
            font.Add(new Document("w.pitch", "w:val", "variable"));

            ns = new Dictionary<string, string>();

            ns.Add("w:usb0", "E00002FF");
            ns.Add("w:usb1", "400004FF");
            ns.Add("w:usb2", "00000000");
            ns.Add("w:usb3", "00000000");
            ns.Add("w:csb0", "0000019F");
            ns.Add("w:csb1", "00000000");

            font.Add(new Document("w:sig", ns));
            #endregion

            f.Add(font);

            f.Done();
        }
    }
}