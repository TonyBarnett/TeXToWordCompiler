using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Xml;

namespace TexWordCompiler
{
    public delegate void SetLabel(string value);

    public partial class Form1 : Form
    {
        private Reader _Reader;
        private FileInfo _File;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (filenames.Count() != 1)
            {
                textBox1.Text = string.Format("Please drag/ drop 1 file, I make this {0}", filenames.Count());
            }

            else if (filenames[0].Substring(filenames[0].Length - 3).ToLower() != "tex")
            {
                label1.Text = "Please enter valid TeX file";
            }
            else
            {
                textBox1.Text = filenames[0];
                _Reader = new Reader(textBox1.Text);
                _Reader.Sl += new SetLabel(SetLabel);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _File = new FileInfo(textBox1.Text);

            if (_File.Extension.ToLower() != ".tex")
            {
                label1.Text = "Please enter valid TeX file";
            }
            else
            {
                label1.Text = "working...";

                Writer w = new Writer(_File.DirectoryName, _File.Name);
                Thread t = new Thread(new ThreadStart(MainLoop));
                t.Start();
            }
        }

        private void SetLabel(string label)
        {
            if (InvokeRequired)
            {
                Invoke(new SetLabel(SetLabel), new object[] { label });
            }
            else
            {
                label1.Text = string.Format(label);
            }
        }

        private void MainLoop()
        {
            Reader r = _Reader;
            FileInfo f = _File;
            Writer w = new Writer(f.DirectoryName, f.Name);
            List<Reader.Header> headerInfo = new List<Reader.Header>();

            while (!Reader.Header.IsEnd)
            {
                Reader.Header h = _Reader.GetHeaderInformation();
                if (h != null)
                {
                    headerInfo.Add(h);
                }
                //w.WriteFile(string.Format("{0}\\test.txt", f.Name.Split('.')[0]), output);
            }

            while (!r.EndOfStream)
            {
                string ything = r.ParseLine();
            }

            #region xmlTest
            Document topXml = new Document("root");

            foreach (Reader.Header h in headerInfo)
            {
                Document element = new Document("element", "Type", h.CommandType);

                if (h.Optionals != null)
                {
                    int i = 0;
                    foreach (string s in h.Optionals)
                    {
                        element.AddAttribute("optionalAttribute" + i++.ToString(), s);
                    }
                }

                if (h.Macro != null)
                {
                    foreach (string s in h.Macro.Keys)
                    {
                        Document macro = new Document("Macro", "Input", s, h.Macro[s]);
                        element.Add(macro);
                    }
                }

                Document value = new Document("Output");
                if(h.Values != null)
                    value.SetValue(string.Format("{0}", h.Values[0]));
                element.Add(value);
                 
                topXml.Add(element);
            }

            using (StreamWriter writer = new StreamWriter(string.Format("{0}\\test.xml",f.DirectoryName)))
            {
                writer.Write(topXml.GetXml());
            }
            #endregion

            //Time to do something about the header information that's stored in headerInfo,
            //I advise putting something in the TeX class to work out what they mean then use that to
            //spit stuff out to writer
            //Also you may want to sort out "renewCommand"s... The reader spits them out funny
            //I think the renewCommands thing is sorted, prints to Xml ok
        }
    }
}