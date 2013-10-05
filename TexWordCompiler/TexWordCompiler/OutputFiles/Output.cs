using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace TexWordCompiler.OutputFiles
{
    class Output
    {
        public Output()
        {
            Application app = new Application();

            app.NewWindow();
        }
    }
}
