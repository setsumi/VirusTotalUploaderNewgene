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
            this.labelClear = new DarkUI.Controls.DarkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tmrRateLimiter = new System.Windows.Forms.Timer(this.components);
            this.queueLabel = new DarkUI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // moreLabel
            // 
            this.moreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moreLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moreLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.moreLabel.Location = new System.Drawing.Point(632, 9);
            this.moreLabel.Name = "moreLabel";
            this.moreLabel.Size = new System.Drawing.Size(100, 23);
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
            this.panelUploads.Location = new System.Drawing.Point(15, 38);
            this.panelUploads.Name = "panelUploads";
            this.panelUploads.Size = new System.Drawing.Size(717, 311);
            this.panelUploads.TabIndex = 2;
            this.panelUploads.Resize += new System.EventHandler(this.panelUploads_Resize);
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelMessage.Location = new System.Drawing.Point(12, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(322, 23);
            this.labelMessage.TabIndex = 6;
            this.labelMessage.Text = "drag file here";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMessage.Click += new System.EventHandler(this.labelMessage_Click);
            // 
            // labelClear
            // 
            this.labelClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelClear.Location = new System.Drawing.Point(340, 9);
            this.labelClear.Name = "labelClear";
            this.labelClear.Size = new System.Drawing.Size(71, 23);
            this.labelClear.TabIndex = 7;
            this.labelClear.Text = "clear";
            this.labelClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.labelClear, "Remove all uploads");
            this.labelClear.Click += new System.EventHandler(this.labelClear_Click);
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
            this.queueLabel.Location = new System.Drawing.Point(453, 14);
            this.queueLabel.Name = "queueLabel";
            this.queueLabel.Size = new System.Drawing.Size(63, 13);
            this.queueLabel.TabIndex = 8;
            this.queueLabel.Text = "queueLabel";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 361);
            this.Controls.Add(this.queueLabel);
            this.Controls.Add(this.moreLabel);
            this.Controls.Add(this.panelUploads);
            this.Controls.Add(this.labelClear);
            this.Controls.Add(this.labelMessage);
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
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkLabel moreLabel;
        private DarkUI.Controls.DarkLabel labelMessage;
        private DarkUI.Controls.DarkLabel labelClear;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer tmrRateLimiter;
        private DarkUI.Controls.DarkLabel queueLabel;
        public System.Windows.Forms.Panel panelUploads;
    }
}

