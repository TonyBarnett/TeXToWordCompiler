using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xml;

namespace TexWordCompiler.OutputFiles
{
    class Theme
    {
        public Theme(DirectoryInfo doc)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            namespaces.Add("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            OutputFile f = new OutputFile(namespaces, new FileInfo(doc.FullName + "\\word\\theme\\theme1.xml"), "theme", "a");

            f.AddAttribute("name", "Office Theme");

            Document themeElements = new Document("themeElements", "name", "theme1");
            f.Add(themeElements);
            Document clrScheme = new Document("clrScheme");
            themeElements.Add(clrScheme);

            Document dk1 = new Document("dk1");
            clrScheme.Add(dk1);
            dk1.Add(new Document("schemeClr", "val", "windowText"));
            Document lt1 = new Document("lt1");
            clrScheme.Add(lt1);
            lt1.Add(new Document("schemeClr", "val", "window"));
            Document dk2 = new Document("dk2");
            clrScheme.Add(dk2);
            dk2.Add(new Document("srgbClr", "val", "000000"));
            Document lt2 = new Document("lt2");
            clrScheme.Add(lt2);
            lt2.Add(new Document("srgbClr", "val", "000000"));
            Document accent1 = new Document("accent1");
            clrScheme.Add(accent1);
            accent1.Add(new Document("srgbClr", "val", "000000"));
            Document accent2 = new Document("accent2");
            clrScheme.Add(accent2);
            accent2.Add(new Document("srgbClr", "val", "000000"));
            Document accent3 = new Document("accent3");
            clrScheme.Add(accent3);
            accent3.Add(new Document("srgbClr", "val", "000000"));
            Document accent4 = new Document("accent4");
            clrScheme.Add(accent4);
            accent4.Add(new Document("srgbClr", "val", "000000"));
            Document accent5 = new Document("accent5");
            clrScheme.Add(accent5);
            accent5.Add(new Document("srgbClr", "val", "000000"));
            Document accent6 = new Document("accent6");
            clrScheme.Add(accent6);
            accent6.Add(new Document("srgbClr", "val", "000000"));
            Document hlink = new Document("hlink");
            clrScheme.Add(hlink);
            hlink.Add(new Document("srgbClr", "val", "000000"));
            Document folHlink = new Document("folHlink");
            clrScheme.Add(folHlink);
            folHlink.Add(new Document("srgbClr", "val", "000000"));

            Document fontScheme = new Document("fontScheme", "name", "font1");
            themeElements.Add(fontScheme);
            Document majorFont = new Document("majorFont");
            fontScheme.Add(majorFont);

            majorFont.Add(new Document("latin", "typeface", "Cambria"));
            majorFont.Add(new Document("ea", "typeface", ""));
            majorFont.Add(new Document("cs", "typeface", ""));
            
            Document minorFont = new Document("minorFont");
            fontScheme.Add(minorFont);

            minorFont.Add(new Document("latin", "typeface", "Calibri"));
            minorFont.Add(new Document("ea", "typeface", ""));
            minorFont.Add(new Document("cs", "typeface", ""));

            Document fmtScheme = new Document("fmtScheme", "name", "Office");
            themeElements.Add(fmtScheme);
            Document fillStyleList = new Document("a:fillStyleLst");
            fmtScheme.Add(fillStyleList);

            Document solidFill = new Document("a:solidFill");
            fillStyleList.Add(solidFill);
            solidFill.Add(new Document("a:schemeClr", "val", "phClr"));

            Document gradFill = new Document("a:gradFill", "rotWithShape", "true");
            fillStyleList.Add(gradFill);

            Document blipFill = new Document("a:blipFill", "rotWithShape", "true");
            fillStyleList.Add(blipFill);

            Document lineStyle = new Document("a:lnStyleLst");
            fmtScheme.Add(lineStyle);

            lineStyle.Add(new Document("a:ln"));
            lineStyle.Add(new Document("a:ln"));
            lineStyle.Add(new Document("a:ln"));

            Document effectStyleList = new Document("a:effectStyleLst");
            fmtScheme.Add(effectStyleList);

            effectStyleList.Add(new Document("a:effectList"));
            effectStyleList.Add(new Document("a:effectDrag"));

            Document bgFillStyleLst = new Document("a:bgFillStyleLst");
            bgFillStyleLst.Add(solidFill);
            bgFillStyleLst.Add(gradFill);
            bgFillStyleLst.Add(blipFill);

            f.Done();
        }
    }
}
