namespace uploader
{
    partial class UploadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadForm));
            this.fileGroup = new DarkUI.Controls.DarkGroupBox();
            this.statusLabel = new DarkUI.Controls.DarkLabel();
            this.sha2Textbox = new DarkUI.Controls.DarkTextBox();
            this.uploadButton = new DarkUI.Controls.DarkButton();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.folderTextbox = new DarkUI.Controls.DarkTextBox();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.fileTextbox = new DarkUI.Controls.DarkTextBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fileGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileGroup
            // 
            this.fileGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fileGroup.Controls.Add(this.statusLabel);
            this.fileGroup.Controls.Add(this.sha2Textbox);
            this.fileGroup.Controls.Add(this.uploadButton);
            this.fileGroup.Controls.Add(this.darkLabel3);
            this.fileGroup.Controls.Add(this.folderTextbox);
            this.fileGroup.Controls.Add(this.darkLabel2);
            this.fileGroup.Controls.Add(this.fileTextbox);
            this.fileGroup.Controls.Add(this.darkLabel1);
            this.fileGroup.Location = new System.Drawing.Point(13, 13);
            this.fileGroup.Name = "fileGroup";
            this.fileGroup.Size = new System.Drawing.Size(551, 129);
            this.fileGroup.TabIndex = 0;
            this.fileGroup.TabStop = false;
            this.fileGroup.Text = "information";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.statusLabel.Location = new System.Drawing.Point(90, 103);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(23, 13);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "idle";
            this.statusLabel.Click += new System.EventHandler(this.statusLabel_Click);
            // 
            // sha2Textbox
            // 
            this.sha2Textbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sha2Textbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.sha2Textbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sha2Textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.sha2Textbox.Location = new System.Drawing.Point(73, 72);
            this.sha2Textbox.Name = "sha2Textbox";
            this.sha2Textbox.ReadOnly = true;
            this.sha2Textbox.Size = new System.Drawing.Size(472, 20);
            this.sha2Textbox.TabIndex = 5;
            // 
            // uploadButton
            // 
            this.uploadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadButton.Location = new System.Drawing.Point(9, 98);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Padding = new System.Windows.Forms.Padding(5);
            this.uploadButton.Size = new System.Drawing.Size(75, 23);
            this.uploadButton.TabIndex = 1;
            this.uploadButton.Text = "upload";
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // darkLabel3
            // 
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel3.Location = new System.Drawing.Point(6, 74);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(50, 13);
            this.darkLabel3.TabIndex = 4;
            this.darkLabel3.Text = "SHA256:";
            // 
            // folderTextbox
            // 
            this.folderTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.folderTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folderTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.folderTextbox.Location = new System.Drawing.Point(73, 46);
            this.folderTextbox.Name = "folderTextbox";
            this.folderTextbox.ReadOnly = true;
            this.folderTextbox.Size = new System.Drawing.Size(472, 20);
            this.folderTextbox.TabIndex = 3;
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(6, 48);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(39, 13);
            this.darkLabel2.TabIndex = 2;
            this.darkLabel2.Text = "Folder:";
            // 
            // fileTextbox
            // 
            this.fileTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.fileTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fileTextbox.Location = new System.Drawing.Point(73, 20);
            this.fileTextbox.Name = "fileTextbox";
            this.fileTextbox.ReadOnly = true;
            this.fileTextbox.Size = new System.Drawing.Size(472, 20);
            this.fileTextbox.TabIndex = 1;
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(6, 22);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(26, 13);
            this.darkLabel1.TabIndex = 0;
            this.darkLabel1.Text = "File:";
            // 
            // UploadForm
            // 
            this.AcceptButton = this.uploadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 148);
            this.Controls.Add(this.fileGroup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UploadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VirusTotal Uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UploadForm_FormClosing);
            this.Load += new System.EventHandler(this.UploadForm_Load);
            this.fileGroup.ResumeLayout(false);
            this.fileGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox fileGroup;
        private DarkUI.Controls.DarkButton uploadButton;
        private DarkUI.Controls.DarkLabel statusLabel;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkTextBox sha2Textbox;
        private DarkUI.Controls.DarkLabel darkLabel3;
        private DarkUI.Controls.DarkTextBox folderTextbox;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkTextBox fileTextbox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}