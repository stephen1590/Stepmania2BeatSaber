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
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(404, 338);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Open Output";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Enabled = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Stepmania2BeatSaberUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(537, 373);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.consoleOutputWindow);
            this.Controls.Add(this.fileBox);
            this.Controls.Add(this.ExecuteButton);
            this.Controls.Add(this.LoadFileButton);
            this.Name = "Stepmania2BeatSaberUI";
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
    }
}