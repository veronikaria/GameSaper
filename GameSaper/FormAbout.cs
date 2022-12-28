using System;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp.Interfaces;

namespace WindowsFormsApp
{
    public partial class FormAbout : Form, IFileOperations
    {
        public FormAbout()
        {
            InitializeComponent();
            Read();
        }

        public void Read()
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader(FormGame.file);
                line = sr.ReadLine();
                while (line != null)
                {
                    this.fileRichTextBox.Text += line;
                    this.fileRichTextBox.Text += "\n";
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Виникла помилка: " + e.Message);
            }
        }

        public void WriteTo() { }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
