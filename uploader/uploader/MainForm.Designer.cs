namespace uploader
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.moreLabel = new DarkUI.Controls.DarkLabel();
            this.panelUploads = new System.Windows.Forms.Panel();
            this.labelMessage = new DarkUI.Controls.DarkLabel();
            this.tmrRateLimiter = new System.Windows.Forms.Timer(this.components);
            this.queueLabel = new DarkUI.Controls.DarkLabel();
            this.selAll = new DarkUI.Controls.DarkLabel();
            this.selDetected = new DarkUI.Controls.DarkLabel();
            this.selUndetected = new DarkUI.Controls.DarkLabel();
            this.selUploaded = new DarkUI.Controls.DarkLabel();
            this.selError = new DarkUI.Controls.DarkLabel();
            this.selAborted = new DarkUI.Controls.DarkLabel();
            this.selIdle = new DarkUI.Controls.DarkLabel();
            this.selUploading = new DarkUI.Controls.DarkLabel();
            this.contextMenu1 = new DarkUI.Controls.DarkContextMenu();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doAllLabel = new DarkUI.Controls.DarkLabel();
            this.selChecking = new DarkUI.Controls.DarkLabel();
            this.labelSounds = new DarkUI.Controls.DarkLabel();
            this.contextMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // moreLabel
            // 
            this.moreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moreLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moreLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.moreLabel.Location = new System.Drawing.Point(698, 9);
            this.moreLabel.Name = "moreLabel";
            this.moreLabel.Size = new System.Drawing.Size(62, 23);
            this.moreLabel.TabIndex = 1;
            this.moreLabel.Text = "setting";
            this.moreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.moreLabel.Click += new System.EventHandler(this.moreLabel_Click);
            // 
            // panelUploads
            // 
            this.panelUploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUploads.AutoScroll = true;
            this.panelUploads.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.panelUploads.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.panelUploads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUploads.Location = new System.Drawing.Point(12, 38);
            this.panelUploads.Name = "panelUploads";
            this.panelUploads.Size = new System.Drawing.Size(748, 325);
            this.panelUploads.TabIndex = 2;
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelMessage.Location = new System.Drawing.Point(12, 366);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(82, 13);
            this.labelMessage.TabIndex = 6;
            this.labelMessage.Text = "drag file here";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelMessage.Click += new System.EventHandler(this.labelMessage_Click);
            // 
            // tmrRateLimiter
            // 
            this.tmrRateLimiter.Interval = 1000;
            this.tmrRateLimiter.Tick += new System.EventHandler(this.tmrRateLimiter_Tick);
            // 
            // queueLabel
            // 
            this.queueLabel.AutoSize = true;
            this.queueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.queueLabel.Location = new System.Drawing.Point(613, 14);
            this.queueLabel.Name = "queueLabel";
            this.queueLabel.Size = new System.Drawing.Size(63, 13);
            this.queueLabel.TabIndex = 8;
            this.queueLabel.Text = "queueLabel";
            // 
            // selAll
            // 
            this.selAll.AutoSize = true;
            this.selAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selAll.Location = new System.Drawing.Point(12, 14);
            this.selAll.Name = "selAll";
            this.selAll.Size = new System.Drawing.Size(31, 13);
            this.selAll.TabIndex = 9;
            this.selAll.Text = "selAll";
            this.selAll.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selDetected
            // 
            this.selDetected.AutoSize = true;
            this.selDetected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selDetected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selDetected.Location = new System.Drawing.Point(92, 14);
            this.selDetected.Name = "selDetected";
            this.selDetected.Size = new System.Drawing.Size(64, 13);
            this.selDetected.TabIndex = 10;
            this.selDetected.Text = "selDetected";
            this.selDetected.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selUndetected
            // 
            this.selUndetected.AutoSize = true;
            this.selUndetected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selUndetected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selUndetected.Location = new System.Drawing.Point(162, 14);
            this.selUndetected.Name = "selUndetected";
            this.selUndetected.Size = new System.Drawing.Size(76, 13);
            this.selUndetected.TabIndex = 11;
            this.selUndetected.Text = "selUndetected";
            this.selUndetected.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selUploaded
            // 
            this.selUploaded.AutoSize = true;
            this.selUploaded.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selUploaded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selUploaded.Location = new System.Drawing.Point(389, 14);
            this.selUploaded.Name = "selUploaded";
            this.selUploaded.Size = new System.Drawing.Size(66, 13);
            this.selUploaded.TabIndex = 12;
            this.selUploaded.Text = "selUploaded";
            this.selUploaded.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selError
            // 
            this.selError.AutoSize = true;
            this.selError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selError.Location = new System.Drawing.Point(461, 14);
            this.selError.Name = "selError";
            this.selError.Size = new System.Drawing.Size(42, 13);
            this.selError.TabIndex = 13;
            this.selError.Text = "selError";
            this.selError.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selAborted
            // 
            this.selAborted.AutoSize = true;
            this.selAborted.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selAborted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selAborted.Location = new System.Drawing.Point(509, 14);
            this.selAborted.Name = "selAborted";
            this.selAborted.Size = new System.Drawing.Size(57, 13);
            this.selAborted.TabIndex = 14;
            this.selAborted.Text = "selAborted";
            this.selAborted.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selIdle
            // 
            this.selIdle.AutoSize = true;
            this.selIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selIdle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selIdle.Location = new System.Drawing.Point(49, 14);
            this.selIdle.Name = "selIdle";
            this.selIdle.Size = new System.Drawing.Size(37, 13);
            this.selIdle.TabIndex = 15;
            this.selIdle.Text = "selIdle";
            this.selIdle.Click += new System.EventHandler(this.selAll_Click);
            // 
            // selUploading
            // 
            this.selUploading.AutoSize = true;
            this.selUploading.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selUploading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selUploading.Location = new System.Drawing.Point(315, 14);
            this.selUploading.Name = "selUploading";
            this.selUploading.Size = new System.Drawing.Size(68, 13);
            this.selUploading.TabIndex = 16;
            this.selUploading.Text = "selUploading";
            this.selUploading.Click += new System.EventHandler(this.selAll_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.contextMenu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.contextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clearToolStripMenuItem});
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.Size = new System.Drawing.Size(117, 55);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.clearToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // doAllLabel
            // 
            this.doAllLabel.AutoSize = true;
            this.doAllLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doAllLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.doAllLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.doAllLabel.Location = new System.Drawing.Point(572, 14);
            this.doAllLabel.Name = "doAllLabel";
            this.doAllLabel.Size = new System.Drawing.Size(35, 13);
            this.doAllLabel.TabIndex = 17;
            this.doAllLabel.Text = "Do All";
            this.doAllLabel.Click += new System.EventHandler(this.doAllLabel_Click);
            // 
            // selChecking
            // 
            this.selChecking.AutoSize = true;
            this.selChecking.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selChecking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.selChecking.Location = new System.Drawing.Point(244, 14);
            this.selChecking.Name = "selChecking";
            this.selChecking.Size = new System.Drawing.Size(65, 13);
            this.selChecking.TabIndex = 18;
            this.selChecking.Text = "selChecking";
            this.selChecking.Click += new System.EventHandler(this.selAll_Click);
            // 
            // labelSounds
            // 
            this.labelSounds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSounds.AutoSize = true;
            this.labelSounds.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelSounds.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelSounds.Location = new System.Drawing.Point(698, 366);
            this.labelSounds.Name = "labelSounds";
            this.labelSounds.Size = new System.Drawing.Size(49, 13);
            this.labelSounds.TabIndex = 19;
            this.labelSounds.Text = " Sounds ";
            this.labelSounds.Click += new System.EventHandler(this.labelSounds_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 383);
            this.Controls.Add(this.labelSounds);
            this.Controls.Add(this.selChecking);
            this.Controls.Add(this.doAllLabel);
            this.Controls.Add(this.queueLabel);
            this.Controls.Add(this.selUploading);
            this.Controls.Add(this.selIdle);
            this.Controls.Add(this.selAborted);
            this.Controls.Add(this.selError);
            this.Controls.Add(this.selUploaded);
            this.Controls.Add(this.selUndetected);
            this.Controls.Add(this.selDetected);
            this.Controls.Add(this.selAll);
            this.Controls.Add(this.panelUploads);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.moreLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VirusTotal Uploader Newgene";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenu1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkLabel moreLabel;
        private DarkUI.Controls.DarkLabel labelMessage;
        private System.Windows.Forms.Timer tmrRateLimiter;
        private DarkUI.Controls.DarkLabel queueLabel;
        public System.Windows.Forms.Panel panelUploads;
        private DarkUI.Controls.DarkLabel selAll;
        private DarkUI.Controls.DarkLabel selDetected;
        private DarkUI.Controls.DarkLabel selUndetected;
        private DarkUI.Controls.DarkLabel selUploaded;
        private DarkUI.Controls.DarkLabel selError;
        private DarkUI.Controls.DarkLabel selAborted;
        private DarkUI.Controls.DarkLabel selIdle;
        private DarkUI.Controls.DarkLabel selUploading;
        private DarkUI.Controls.DarkContextMenu contextMenu1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private DarkUI.Controls.DarkLabel doAllLabel;
        private DarkUI.Controls.DarkLabel selChecking;
        private DarkUI.Controls.DarkLabel labelSounds;
    }
}

