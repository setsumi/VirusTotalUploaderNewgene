namespace uploader
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.generalGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.darkLabel4 = new DarkUI.Controls.DarkLabel();
            this.minSizeUpDown = new DarkUI.Controls.DarkNumericUpDown();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.callsPerMinuteUpDown = new DarkUI.Controls.DarkNumericUpDown();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.apiLimitsLabel = new DarkUI.Controls.DarkLabel();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.directCheckbox = new DarkUI.Controls.DarkCheckBox();
            this.languageCombo = new DarkUI.Controls.DarkComboBox();
            this.languageLabel = new DarkUI.Controls.DarkLabel();
            this.getApiButton = new DarkUI.Controls.DarkButton();
            this.apiTextbox = new DarkUI.Controls.DarkTextBox();
            this.apiLabel = new DarkUI.Controls.DarkLabel();
            this.saveButton = new DarkUI.Controls.DarkButton();
            this.openButton = new DarkUI.Controls.DarkButton();
            this.statusLabel = new DarkUI.Controls.DarkLabel();
            this.generalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.callsPerMinuteUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.generalGroupBox.Controls.Add(this.darkLabel4);
            this.generalGroupBox.Controls.Add(this.minSizeUpDown);
            this.generalGroupBox.Controls.Add(this.darkLabel3);
            this.generalGroupBox.Controls.Add(this.callsPerMinuteUpDown);
            this.generalGroupBox.Controls.Add(this.darkLabel2);
            this.generalGroupBox.Controls.Add(this.apiLimitsLabel);
            this.generalGroupBox.Controls.Add(this.darkLabel1);
            this.generalGroupBox.Controls.Add(this.directCheckbox);
            this.generalGroupBox.Controls.Add(this.languageCombo);
            this.generalGroupBox.Controls.Add(this.languageLabel);
            this.generalGroupBox.Controls.Add(this.getApiButton);
            this.generalGroupBox.Controls.Add(this.apiTextbox);
            this.generalGroupBox.Controls.Add(this.apiLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(13, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(483, 206);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "settings";
            // 
            // darkLabel4
            // 
            this.darkLabel4.AutoSize = true;
            this.darkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel4.Location = new System.Drawing.Point(230, 176);
            this.darkLabel4.Name = "darkLabel4";
            this.darkLabel4.Size = new System.Drawing.Size(33, 13);
            this.darkLabel4.TabIndex = 12;
            this.darkLabel4.Text = "Bytes";
            // 
            // minSizeUpDown
            // 
            this.minSizeUpDown.Location = new System.Drawing.Point(164, 174);
            this.minSizeUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.minSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minSizeUpDown.Name = "minSizeUpDown";
            this.minSizeUpDown.Size = new System.Drawing.Size(60, 20);
            this.minSizeUpDown.TabIndex = 11;
            this.minSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // darkLabel3
            // 
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel3.Location = new System.Drawing.Point(32, 176);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(117, 13);
            this.darkLabel3.TabIndex = 10;
            this.darkLabel3.Text = "Ignore files smaller than";
            // 
            // callsPerMinuteUpDown
            // 
            this.callsPerMinuteUpDown.Location = new System.Drawing.Point(164, 142);
            this.callsPerMinuteUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.callsPerMinuteUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.callsPerMinuteUpDown.Name = "callsPerMinuteUpDown";
            this.callsPerMinuteUpDown.Size = new System.Drawing.Size(60, 20);
            this.callsPerMinuteUpDown.TabIndex = 7;
            this.callsPerMinuteUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(15, 144);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(143, 13);
            this.darkLabel2.TabIndex = 6;
            this.darkLabel2.Text = "Limit API requests per minute";
            // 
            // apiLimitsLabel
            // 
            this.apiLimitsLabel.AutoSize = true;
            this.apiLimitsLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.apiLimitsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.apiLimitsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.apiLimitsLabel.Location = new System.Drawing.Point(305, 144);
            this.apiLimitsLabel.Name = "apiLimitsLabel";
            this.apiLimitsLabel.Size = new System.Drawing.Size(49, 13);
            this.apiLimitsLabel.TabIndex = 9;
            this.apiLimitsLabel.Text = "API limits";
            this.apiLimitsLabel.Click += new System.EventHandler(this.apiLimitsLabel_Click);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(240, 144);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(59, 13);
            this.darkLabel1.TabIndex = 8;
            this.darkLabel1.Text = "consult the";
            // 
            // directCheckbox
            // 
            this.directCheckbox.Location = new System.Drawing.Point(13, 122);
            this.directCheckbox.Name = "directCheckbox";
            this.directCheckbox.Size = new System.Drawing.Size(221, 16);
            this.directCheckbox.TabIndex = 5;
            this.directCheckbox.Text = "immediate upload";
            // 
            // languageCombo
            // 
            this.languageCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.languageCombo.FormattingEnabled = true;
            this.languageCombo.Location = new System.Drawing.Point(74, 85);
            this.languageCombo.Name = "languageCombo";
            this.languageCombo.Size = new System.Drawing.Size(280, 21);
            this.languageCombo.TabIndex = 4;
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.languageLabel.Location = new System.Drawing.Point(10, 88);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(58, 13);
            this.languageLabel.TabIndex = 3;
            this.languageLabel.Text = "Language:";
            // 
            // getApiButton
            // 
            this.getApiButton.Location = new System.Drawing.Point(10, 45);
            this.getApiButton.Name = "getApiButton";
            this.getApiButton.Padding = new System.Windows.Forms.Padding(5);
            this.getApiButton.Size = new System.Drawing.Size(161, 23);
            this.getApiButton.TabIndex = 2;
            this.getApiButton.Text = "Get API key...";
            this.getApiButton.Click += new System.EventHandler(this.getApiButton_Click);
            // 
            // apiTextbox
            // 
            this.apiTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.apiTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.apiTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.apiTextbox.Location = new System.Drawing.Point(60, 19);
            this.apiTextbox.Name = "apiTextbox";
            this.apiTextbox.Size = new System.Drawing.Size(417, 20);
            this.apiTextbox.TabIndex = 1;
            // 
            // apiLabel
            // 
            this.apiLabel.AutoSize = true;
            this.apiLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.apiLabel.Location = new System.Drawing.Point(7, 21);
            this.apiLabel.Name = "apiLabel";
            this.apiLabel.Size = new System.Drawing.Size(47, 13);
            this.apiLabel.TabIndex = 0;
            this.apiLabel.Text = "API key:";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.Location = new System.Drawing.Point(13, 224);
            this.saveButton.Name = "saveButton";
            this.saveButton.Padding = new System.Windows.Forms.Padding(5);
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openButton.Location = new System.Drawing.Point(94, 224);
            this.openButton.Name = "openButton";
            this.openButton.Padding = new System.Windows.Forms.Padding(5);
            this.openButton.Size = new System.Drawing.Size(153, 23);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "explore settings file";
            this.openButton.Click += new System.EventHandler(this.darkButton1_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.statusLabel.Location = new System.Drawing.Point(253, 229);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(74, 13);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "no settings file";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 259);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.generalGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.callsPerMinuteUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox generalGroupBox;
        private DarkUI.Controls.DarkTextBox apiTextbox;
        private DarkUI.Controls.DarkLabel apiLabel;
        private DarkUI.Controls.DarkButton getApiButton;
        private DarkUI.Controls.DarkLabel languageLabel;
        private DarkUI.Controls.DarkComboBox languageCombo;
        private DarkUI.Controls.DarkButton saveButton;
        private DarkUI.Controls.DarkButton openButton;
        private DarkUI.Controls.DarkCheckBox directCheckbox;
        private DarkUI.Controls.DarkLabel statusLabel;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel apiLimitsLabel;
        private DarkUI.Controls.DarkNumericUpDown callsPerMinuteUpDown;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkLabel darkLabel4;
        private DarkUI.Controls.DarkNumericUpDown minSizeUpDown;
        private DarkUI.Controls.DarkLabel darkLabel3;
    }
}