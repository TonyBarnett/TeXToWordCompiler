using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TexWordCompiler.OutputFiles
{
    class WebSettings : StreamWriter
    {
        public WebSettings(DirectoryInfo doc)
            : base(doc.FullName + "\\word\\webSettings.xml")
        {
            Write("<?xmlversion=\"1.0\"encoding=\"UTF-8\"standalone=\"yes\"?>");
            Write("<w:webSettingsxmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            Write("<w:optimizeForBrowser/>");
            Write("</w:webSettings>");
        }
    }
}
