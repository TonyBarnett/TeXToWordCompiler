using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class Settings
    {
        public Settings(DirectoryInfo doc)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("0", "urn:schemas-microsoft-com:office:office");
            namespaces.Add("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            namespaces.Add("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            namespaces.Add("v", "urn:schemas-microsoft-com:vml");
            namespaces.Add("w10", "urn:schemas-microsoft-com:office:word");
            namespaces.Add("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            namespaces.Add("sl", "http://schemas.openxmlformats.org/schemaLibrary/2006/main");

            OutputFile f = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\word\\settings.xml"), "fonts", "w");

            f.Add(new Xml.Document("w:zoom", "w:percent", "100"));
            f.Add(new Xml.Document("w:defaultTabStop", "w:val", "720"));
            f.Add(new Xml.Document("w:characterSpacingControl", "w:val", "doNotCompress"));
            f.Add(new Xml.Document("w:compat"));

            Xml.Document rsid = new Xml.Document("w:rsids");

            rsid.Add(new Xml.Document("w:rsidRoot", "w:val", "00000000"));
            rsid.Add(new Xml.Document("w:rsid", "w:val", "00000000"));

            f.Add(rsid);

            Xml.Document mathPr = new Xml.Document("m:mathPr");
            mathPr.Add(new Xml.Document("m:mathFont", "m:val", "Cambria Math"));
            mathPr.Add(new Xml.Document("m:brkBin", "m:val", "before"));
            mathPr.Add(new Xml.Document("m:brkBinSub", "m:val", "--"));
            mathPr.Add(new Xml.Document("m:smallFrac", "m:val", "off"));
            mathPr.Add(new Xml.Document("m:dispDef"));
            mathPr.Add(new Xml.Document("m:lMargin", "m:val", "0"));
            mathPr.Add(new Xml.Document("m:rMargin", "m:val", "0"));
            mathPr.Add(new Xml.Document("m:defJc", "m:val", "centerGroup"));
            mathPr.Add(new Xml.Document("m:wrapIndent", "m:val", "1440"));
            mathPr.Add(new Xml.Document("m:intLim", "m:val", "subSup"));
            mathPr.Add(new Xml.Document("m:naryLim", "m:val", "undOvr"));

            f.Add(mathPr);

            f.Add(new Xml.Document("w:themeFontLang", "w:val", "en-GB"));
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("w:bg1", "light1");
            attributes.Add("w:t1", "dark1");
            attributes.Add("w:bg2", "light2");
            attributes.Add("w:t2", "dark2");
            attributes.Add("w:accent1", "accent1");
            attributes.Add("w:accent2", "accent2");
            attributes.Add("w:accent3", "accent3");
            attributes.Add("w:accent4", "accent4");
            attributes.Add("w:accent5", "accent5");
            attributes.Add("w:accent6", "accent6");
            attributes.Add("w:hyperlink", "hyperlink");
            attributes.Add("w:followedHyperlink", "followedHyperlink");
            f.Add(new Xml.Document("w:clrSchemeMapping", attributes));

            Xml.Document shapeDefaults = new Xml.Document("w:shapeDefaults");
            attributes = new Dictionary<string, string>();
            attributes.Add("v:ext", "edit");
            attributes.Add("spidmax", "2050");
            shapeDefaults.Add(new Xml.Document("o:shapedefaults", attributes));

            Xml.Document shapeLayout = new Xml.Document("o:shapelayout","v:ext","edit");

            attributes= new Dictionary<string,string>();
            attributes.Add("v:ext", "edit");
            attributes.Add("data", "1");
            shapeLayout.Add(new Xml.Document("o:idmap",attributes));
            shapeDefaults.Add(shapeLayout);

            f.Add(shapeDefaults);
            f.Add(new Xml.Document("w:decimalSymbol","w:val","."));
            f.Add(new Xml.Document("w:listSeparator","w:val","."));

            f.Done();
        }
    }
}
