namespace Stepmania2BeatSaber
{
    partial class Stepmania2BeatSaberUI
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
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
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 41);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(384, 320);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // Stepmania2BeatSaberUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(537, 373);
            this.Controls.Add(this.richTextBox1);
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
        private RichTextBox richTextBox1;
    }
}