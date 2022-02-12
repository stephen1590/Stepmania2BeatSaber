namespace Stepmania2BeatSaber
{
    partial class UserInterface
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
            this.components = new System.ComponentModel.Container();
            this.LoadFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.consoleOutputWindow = new System.Windows.Forms.RichTextBox();
            this.openOutputButton = new System.Windows.Forms.Button();
            this.setBSaberDirBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.fixRepeatsBox = new System.Windows.Forms.CheckBox();
            this.lessConflictsBox = new System.Windows.Forms.CheckBox();
            this.includeObstaclesBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WIPCustomLevelsDir = new System.Windows.Forms.Button();
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
            this.fileBox.Size = new System.Drawing.Size(384, 23);
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
            // openOutputButton
            // 
            this.openOutputButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.openOutputButton.Enabled = false;
            this.openOutputButton.ForeColor = System.Drawing.SystemColors.Control;
            this.openOutputButton.Location = new System.Drawing.Point(404, 338);
            this.openOutputButton.Name = "openOutputButton";
            this.openOutputButton.Size = new System.Drawing.Size(126, 23);
            this.openOutputButton.TabIndex = 5;
            this.openOutputButton.Text = "Open Output";
            this.openOutputButton.UseVisualStyleBackColor = false;
            this.openOutputButton.Click += new System.EventHandler(this.openOutputButton_Click);
            // 
            // saveConfigButton
            // 
            this.saveConfigButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.saveConfigButton.Enabled = false;
            this.saveConfigButton.ForeColor = System.Drawing.SystemColors.Control;
            this.saveConfigButton.Location = new System.Drawing.Point(404, 266);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(126, 23);
            this.saveConfigButton.TabIndex = 6;
            this.saveConfigButton.Text = "Save Config";
            this.saveConfigButton.UseVisualStyleBackColor = false;
            this.saveConfigButton.Click += new System.EventHandler(this.saveConfigButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(408, 133);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            //
            // fixRepeatsBox
            // 
            this.fixRepeatsBox.AutoSize = true;
            this.fixRepeatsBox.ForeColor = System.Drawing.SystemColors.Control;
            this.fixRepeatsBox.Location = new System.Drawing.Point(409, 162);
            this.fixRepeatsBox.Name = "fixRepeatsBox";
            this.fixRepeatsBox.Size = new System.Drawing.Size(85, 19);
            this.fixRepeatsBox.TabIndex = 8;
            this.fixRepeatsBox.Text = "Fix Repeats";
            this.fixRepeatsBox.UseVisualStyleBackColor = true;
            this.fixRepeatsBox.CheckedChanged += new System.EventHandler(this.fixRepeatsBox_CheckedChanged);
            // 
            // lessConflictsBox
            // 
            this.lessConflictsBox.AutoSize = true;
            this.lessConflictsBox.ForeColor = System.Drawing.SystemColors.Control;
            this.lessConflictsBox.Location = new System.Drawing.Point(409, 187);
            this.lessConflictsBox.Name = "lessConflictsBox";
            this.lessConflictsBox.Size = new System.Drawing.Size(98, 19);
            this.lessConflictsBox.TabIndex = 9;
            this.lessConflictsBox.Text = "Less Conflicts";
            this.lessConflictsBox.UseVisualStyleBackColor = true;
            this.lessConflictsBox.CheckedChanged += new System.EventHandler(this.lessConflictsBox_CheckedChanged);
            // 
            // includeObstaclesBox
            // 
            this.includeObstaclesBox.AutoSize = true;
            this.includeObstaclesBox.ForeColor = System.Drawing.SystemColors.Control;
            this.includeObstaclesBox.Location = new System.Drawing.Point(410, 212);
            this.includeObstaclesBox.Name = "includeObstaclesBox";
            this.includeObstaclesBox.Size = new System.Drawing.Size(119, 19);
            this.includeObstaclesBox.TabIndex = 10;
            this.includeObstaclesBox.Text = "Include Obstacles";
            this.includeObstaclesBox.UseVisualStyleBackColor = true;
            this.includeObstaclesBox.CheckedChanged += new System.EventHandler(this.includeObstaclesBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(410, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Configurations";
            // 
            // WIPCustomLevelsDir
            // 
            this.WIPCustomLevelsDir.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.WIPCustomLevelsDir.ForeColor = System.Drawing.SystemColors.Control;
            this.WIPCustomLevelsDir.Location = new System.Drawing.Point(404, 237);
            this.WIPCustomLevelsDir.Name = "WIPCustomLevelsDir";
            this.WIPCustomLevelsDir.Size = new System.Drawing.Size(126, 23);
            this.WIPCustomLevelsDir.TabIndex = 12;
            this.WIPCustomLevelsDir.Text = "Set BSaber Dir";
            this.WIPCustomLevelsDir.UseVisualStyleBackColor = false;
            this.WIPCustomLevelsDir.Click += new System.EventHandler(this.WIPCustomLevelsDir_Click);
            // 
            // SM2BSUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(537, 373);
            this.Controls.Add(this.WIPCustomLevelsDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.includeObstaclesBox);
            this.Controls.Add(this.lessConflictsBox);
            this.Controls.Add(this.fixRepeatsBox);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.saveConfigButton);
            this.Controls.Add(this.openOutputButton);
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
        private Button openOutputButton;
        private FolderBrowserDialog setBSaberDirBrowser;
        private Button saveConfigButton;
        private ComboBox comboBox1;
        private CheckBox fixRepeatsBox;
        private CheckBox lessConflictsBox;
        private CheckBox includeObstaclesBox;
        private Label label1;
        private Button WIPCustomLevelsDir;
    }
}