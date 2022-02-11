namespace Stepmania2BeatSaber
{
    partial class SM2BSUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoadFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.consoleOutputWindow = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadFileButton
            // 
            this.LoadFileButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LoadFileButton.ForeColor = System.Drawing.SystemColors.Control;
            this.LoadFileButton.Location = new System.Drawing.Point(404, 12);
            this.LoadFileButton.Margin = new System.Windows.Forms.Padding(0);
            this.LoadFileButton.Name = "LoadFileButton";
            this.LoadFileButton.Size = new System.Drawing.Size(126, 23);
            this.LoadFileButton.TabIndex = 0;
            this.LoadFileButton.Text = "Load SM File";
            this.LoadFileButton.UseVisualStyleBackColor = false;
            this.LoadFileButton.Click += new System.EventHandler(this.FileBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ExecuteButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.ExecuteButton.Enabled = false;
            this.ExecuteButton.ForeColor = System.Drawing.SystemColors.Control;
            this.ExecuteButton.Location = new System.Drawing.Point(404, 41);
            this.ExecuteButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(126, 23);
            this.ExecuteButton.TabIndex = 1;
            this.ExecuteButton.Text = "Execute";
            this.ExecuteButton.UseVisualStyleBackColor = false;
            this.ExecuteButton.Click += new System.EventHandler(this.Execute_Click);
            // 
            // fileBox
            // 
            this.fileBox.Location = new System.Drawing.Point(12, 12);
            this.fileBox.Name = "fileBox";
            this.fileBox.Size = new System.Drawing.Size(386, 23);
            this.fileBox.TabIndex = 2;
            this.fileBox.TextChanged += new System.EventHandler(this.fileBox_TextChanged);
            // 
            // consoleOutputWindow
            // 
            this.consoleOutputWindow.Location = new System.Drawing.Point(12, 41);
            this.consoleOutputWindow.Name = "consoleOutputWindow";
            this.consoleOutputWindow.Size = new System.Drawing.Size(384, 320);
            this.consoleOutputWindow.TabIndex = 4;
            this.consoleOutputWindow.Text = "";
            this.consoleOutputWindow.TextChanged += new System.EventHandler(this.consoleOutputWindow_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.Enabled = false;
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(404, 338);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Open Output";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.Enabled = false;
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(404, 237);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save Config";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(408, 133);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox1.Location = new System.Drawing.Point(409, 162);
            this.checkBox1.Name = "Fix Repeats";
            this.checkBox1.Size = new System.Drawing.Size(85, 19);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Fix Repeats";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Checked = pOptions.options.ResolveRepeats;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox2.Location = new System.Drawing.Point(409, 187);
            this.checkBox2.Name = "Less Conflicts";
            this.checkBox2.Size = new System.Drawing.Size(98, 19);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Less Conflicts";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Checked = pOptions.options.ResolveConflicts;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox3.Location = new System.Drawing.Point(410, 212);
            this.checkBox3.Name = "Include Obstacles";
            this.checkBox3.Size = new System.Drawing.Size(119, 19);
            this.checkBox3.TabIndex = 10;
            this.checkBox3.Text = "Include Obstacles";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Checked = pOptions.options.ApplyObstacles;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(410, 105);
            this.label1.Name = "Configurations";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Configurations";
            // 
            // SM2BSUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(537, 373);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.consoleOutputWindow);
            this.Controls.Add(this.fileBox);
            this.Controls.Add(this.ExecuteButton);
            this.Controls.Add(this.LoadFileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SM2BSUI";
            this.Text = "Stepmania2BeatSaberUI";
            this.Load += new System.EventHandler(this.Stepmania2BeatSaberUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Button LoadFileButton;
        private OpenFileDialog openFileDialog1;
        private Button ExecuteButton;
        private TextBox fileBox;
        private RichTextBox consoleOutputWindow;
        private Button button1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button2;
        private ComboBox comboBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private Label label1;
    }
}