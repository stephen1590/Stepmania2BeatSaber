using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;

namespace Stepmania2BeatSaber
{
    public partial class Stepmania2BeatSaberUI : Form
    {
        private string pDir { get; set; }//@"C:\src\BeatSaber\BREAK DOWN!\original";
        private string pFilename { get; set; }//"BREAK DOWN!.sm";
        private string pSongName { get; set; }//pFilename.Split(".")[0];
        private string pFullFilePath { get; set; }
        public Stepmania2BeatSaberUI()
        {
            pDir = @"C:\src\BeatSaber";
            pFilename = string.Empty;
            pSongName = string.Empty;
            pFullFilePath = string.Empty;
            InitializeComponent();
        }

        private void FileBrowse_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            if (Directory.Exists(pDir))
            {
                openFileDialog1.InitialDirectory = pDir;
            }
            else
            {
                openFileDialog1.InitialDirectory = @"c:\";
            }
            openFileDialog1.Filter = "All files (*.*)|*.*|sm files (*.sm)|*.sm";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;
                if (filePath != string.Empty)
                {
                    if (!filePath.EndsWith(".sm"))
                    {
                        MessageBox.Show("Not a Stepmania File: " + filePath, filePath, MessageBoxButtons.OK);
                    }
                    else
                    {
                        fileBox.Text = filePath;
                    }
                }
            }
        }

        private void Execute_Click(object sender, EventArgs e)
        {
            if(fileBox.Text == string.Empty)
            {
                MessageBox.Show("Error! No file selected to convert.", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                pFilename = Path.GetFileName(fileBox.Text);
                var d = Path.GetDirectoryName(fileBox.Text);
                if (d != null)
                {
                    pDir = (string)d;
                    if(pFilename != string.Empty && pDir != string.Empty)
                    {
                        double bpm = 0.0;
                        double offset = 0.0;
                        OrderedDictionary rawDAta = Stepmania2BeatSaber.GetRawNotes(pDir, pFilename);
                        if (rawDAta != null && rawDAta.Keys.Count > 0)
                        {
                            var temp = rawDAta["offset"];
                            if (temp != null)
                            {
                                offset = (double)temp;
                                Helper.Output("Offset Found: " + offset.ToString(), ConsoleColor.Green, DebugState.on);
                            }
                            temp = rawDAta["bpm"];
                            if (temp != null)
                            {
                                bpm = (double)temp;
                                Helper.Output("BPM Found: " + bpm.ToString(), ConsoleColor.Green, DebugState.on);
                            }
                            temp = rawDAta["songs"];
                            if (temp != null)
                            {
                                var songs = Stepmania2BeatSaber.CreatBeatSabreEquivalent((OrderedDictionary)temp, offset, bpm);
                                Helper.WriteFile(songs, pDir, pSongName);
                            }
                        }
                    }
                }
               
            }
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Stepmania2BeatSaberUI_Load(object sender, EventArgs e)
        {
            Console.SetOut(new TextBoxWriter(richTextBox1));
        }
    }
    public class TextBoxWriter : TextWriter
    {
        RichTextBox _output = null;

        public TextBoxWriter(RichTextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
