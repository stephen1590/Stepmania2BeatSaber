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
    public partial class UserInterface : Form
    {
        private string pDir = @"C:\src\BeatSaber";
        private string pFilename = String.Empty;
        private string pSongName = String.Empty;
        private readonly SM2BS s = new();
        public UserInterface()
        {
            InitializeComponent();
            optionsSetup();
        }
        private void optionsSetup()
        {
            if (s.o != null && s.o != null)
            {
                comboBox1.DataSource = Enum.GetValues(typeof(GameDifficulty));
                comboBox1.SelectedIndex = (int)s.o.MyGameDifficulty;
                //-----
                fixRepeatsBox.Checked = s.o.ResolveRepeats;
                includeObstaclesBox.Checked = s.o.ApplyObstacles;
                lessConflictsBox.Checked = s.o.ResolveConflicts;
                translateCheckBox.Checked = s.o.TranslatePatterns;
            }

        }
        private void FileBrowse_Click(object sender, EventArgs e)
        {
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
                string filePath = openFileDialog1.FileName;
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
                if (saveConfigButton.Enabled)
                {
                    //we might have new options that the user forgot to commit
                    saveConfig();
                }
                pFilename = Path.GetFileName(fileBox.Text);
                var d = Path.GetDirectoryName(fileBox.Text);
                if (d != null)
                {
                    pDir = (string)d;
                    if(pFilename != string.Empty && pDir != string.Empty)
                    {
                        consoleOutputWindow.Text = "";
                        //---------------------------------
                        pSongName = pFilename.Split(".")[0];
                        double bpm = 0.0;
                        double offset = 0.0;
                        OrderedDictionary rawDAta = s.GetRawNotes(pDir, pFilename);
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
                                var songs = s.CreatBeatSabreEquivalent((OrderedDictionary)temp, offset, bpm);
                                Helper.WriteSongs(songs, pDir, pSongName);
                            }
                            openOutputButton.Enabled = true;
                        }
                    }
                }
               
            }
        }
        private void consoleOutputWindow_TextChanged(object sender, EventArgs e)
        {
        }
        private void Stepmania2BeatSaberUI_Load(object sender, EventArgs e)
        {
            Console.SetOut(new TextBoxWriter(consoleOutputWindow));    
        }
        private void fileBox_TextChanged(object sender, EventArgs e)
        {
            if(fileBox.Text.EndsWith(".sm"))
            {
                ExecuteButton.Enabled = true;
            }
            else
            {
                ExecuteButton.Enabled = false;
            }
        }
        private void openOutputButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(pDir + "\\" + "BeatSaber - " + pSongName))
            {
                System.Diagnostics.Process.Start("explorer.exe", pDir + "\\" + "BeatSaber - " + pSongName);
            }
        }
        private void saveConfigButton_Click(object sender, EventArgs e)
        {
            saveConfig();
        }
        public void saveConfig()
        {
            if (s.o != null)
            {

                s.o.MyGameDifficulty = (GameDifficulty)comboBox1.SelectedIndex;
                s.o.ResolveRepeats = fixRepeatsBox.Checked;
                s.o.ApplyObstacles = includeObstaclesBox.Checked;
                s.o.ResolveConflicts = lessConflictsBox.Checked;
                s.o.TranslatePatterns = translateCheckBox.Checked;
                Helper.optionsSave(ref s.o);
                Console.WriteLine("Config Saved to AppData.");
                saveConfigButton.Enabled = false;
            }
        }
        private void fixRepeatsBox_CheckedChanged(object sender, EventArgs e)
        {
            if(s.o != null)
            {
                saveConfigButton.Enabled = true;
            }
        }
        private void includeObstaclesBox_CheckedChanged(object sender, EventArgs e)
        {
            if (s.o != null)
            {
                saveConfigButton.Enabled = true;
            }
        }
        private void lessConflictsBox_CheckedChanged(object sender, EventArgs e)
        {
            if (s.o != null)
            {
                saveConfigButton.Enabled = true;
            }
        }
        private void WIPCustomLevelsDir_Click(object sender, EventArgs e)
        {
            if (setBSaberDirBrowser.ShowDialog() == DialogResult.OK)
            {
                if (s.o != null && s.o != null && setBSaberDirBrowser.SelectedPath != String.Empty)
                {
                    s.o.WIPCustomLevelsPath = setBSaberDirBrowser.SelectedPath;
                    Console.Write("Beat Saber Directory Selected: ");
                    Console.WriteLine(setBSaberDirBrowser.SelectedPath);
                    saveConfigButton.Enabled = true;
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(s.o != null)
            {
                saveConfigButton.Enabled = true;
            }
        }

        private void translateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (s.o != null)
            {
                saveConfigButton.Enabled = true;
            }
        }
    }
    public class TextBoxWriter : TextWriter
    {
        private readonly RichTextBox _output;
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
