using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Xml;

namespace TexWordCompiler
{
    public delegate void SetLabel(string value);

    public partial class Form1 : Form
    {
        private string _Input;
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
                _Input = textBox1.Text;
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

                //Writer w = new Writer(_File.DirectoryName, _File.Name);
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
            FileInfo f = new FileInfo(textBox1.Text);
            using (Reader r = new Reader(f.FullName))
            {
                r.ParseHeader();
            }
        }
    }
}