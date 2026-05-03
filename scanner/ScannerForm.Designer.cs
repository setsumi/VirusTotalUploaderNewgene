namespace scanner
{
    partial class ScannerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScannerForm));
            this.textBoxWindows = new DarkUI.Controls.DarkTextBox();
            this.buttonScanWindows = new DarkUI.Controls.DarkButton();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.textBoxApp = new DarkUI.Controls.DarkTextBox();
            this.buttonOpenApp = new DarkUI.Controls.DarkButton();
            this.buttonCheckApp = new DarkUI.Controls.DarkButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkFilter = new DarkUI.Controls.DarkCheckBox();
            this.buttonOpenFolder = new DarkUI.Controls.DarkButton();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.buttonUpload = new DarkUI.Controls.DarkButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.labelCheckResult = new DarkUI.Controls.DarkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxWindows
            // 
            this.textBoxWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWindows.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBoxWindows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxWindows.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBoxWindows.Location = new System.Drawing.Point(99, 12);
            this.textBoxWindows.Name = "textBoxWindows";
            this.textBoxWindows.ReadOnly = true;
            this.textBoxWindows.Size = new System.Drawing.Size(393, 20);
            this.textBoxWindows.TabIndex = 8;
            // 
            // buttonScanWindows
            // 
            this.buttonScanWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScanWindows.Location = new System.Drawing.Point(498, 12);
            this.buttonScanWindows.Name = "buttonScanWindows";
            this.buttonScanWindows.Padding = new System.Windows.Forms.Padding(5);
            this.buttonScanWindows.Size = new System.Drawing.Size(70, 23);
            this.buttonScanWindows.TabIndex = 9;
            this.buttonScanWindows.Text = "Scan";
            this.buttonScanWindows.Click += new System.EventHandler(this.buttonScanDll_Click);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(12, 14);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(70, 13);
            this.darkLabel1.TabIndex = 7;
            this.darkLabel1.Text = "System folder";
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(12, 47);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(75, 13);
            this.darkLabel2.TabIndex = 10;
            this.darkLabel2.Text = "Program folder";
            // 
            // textBoxApp
            // 
            this.textBoxApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBoxApp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBoxApp.Location = new System.Drawing.Point(99, 45);
            this.textBoxApp.Name = "textBoxApp";
            this.textBoxApp.Size = new System.Drawing.Size(393, 20);
            this.textBoxApp.TabIndex = 11;
            // 
            // buttonOpenApp
            // 
            this.buttonOpenApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenApp.Location = new System.Drawing.Point(498, 45);
            this.buttonOpenApp.Name = "buttonOpenApp";
            this.buttonOpenApp.Padding = new System.Windows.Forms.Padding(5);
            this.buttonOpenApp.Size = new System.Drawing.Size(29, 23);
            this.buttonOpenApp.TabIndex = 0;
            this.buttonOpenApp.Text = "...";
            this.toolTip1.SetToolTip(this.buttonOpenApp, "Choose program folder\r\nor Drag & Drop");
            this.buttonOpenApp.Click += new System.EventHandler(this.buttonOpenApp_Click);
            // 
            // buttonCheckApp
            // 
            this.buttonCheckApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCheckApp.Location = new System.Drawing.Point(568, 45);
            this.buttonCheckApp.Name = "buttonCheckApp";
            this.buttonCheckApp.Padding = new System.Windows.Forms.Padding(5);
            this.buttonCheckApp.Size = new System.Drawing.Size(104, 23);
            this.buttonCheckApp.TabIndex = 1;
            this.buttonCheckApp.Text = "Check";
            this.buttonCheckApp.Click += new System.EventHandler(this.buttonCheckApp_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(657, 333);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // checkFilter
            // 
            this.checkFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkFilter.AutoSize = true;
            this.checkFilter.Location = new System.Drawing.Point(568, 82);
            this.checkFilter.Name = "checkFilter";
            this.checkFilter.Size = new System.Drawing.Size(104, 17);
            this.checkFilter.TabIndex = 5;
            this.checkFilter.Text = "DLL beside EXE";
            this.toolTip1.SetToolTip(this.checkFilter, "Filter DLL sideloading candidates");
            this.checkFilter.CheckedChanged += new System.EventHandler(this.checkFilter_CheckedChanged);
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFolder.Location = new System.Drawing.Point(227, 78);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Padding = new System.Windows.Forms.Padding(5);
            this.buttonOpenFolder.Size = new System.Drawing.Size(82, 23);
            this.buttonOpenFolder.TabIndex = 3;
            this.buttonOpenFolder.Text = "Open Folder";
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // darkLabel3
            // 
            this.darkLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.darkLabel3.Location = new System.Drawing.Point(315, 83);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(85, 13);
            this.darkLabel3.TabIndex = 4;
            this.darkLabel3.Text = "or double click ↓";
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Location = new System.Drawing.Point(417, 78);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Padding = new System.Windows.Forms.Padding(5);
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 12;
            this.buttonUpload.Text = "Upload";
            this.toolTip1.SetToolTip(this.buttonUpload, "Send selection to VirusTotal Uploader Newgene");
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // labelCheckResult
            // 
            this.labelCheckResult.AutoSize = true;
            this.labelCheckResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelCheckResult.Location = new System.Drawing.Point(12, 83);
            this.labelCheckResult.Name = "labelCheckResult";
            this.labelCheckResult.Size = new System.Drawing.Size(90, 13);
            this.labelCheckResult.TabIndex = 13;
            this.labelCheckResult.Text = "labelCheckResult";
            this.labelCheckResult.Visible = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 450);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.checkFilter);
            this.Controls.Add(this.darkLabel3);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonCheckApp);
            this.Controls.Add(this.buttonOpenApp);
            this.Controls.Add(this.textBoxApp);
            this.Controls.Add(this.darkLabel2);
            this.Controls.Add(this.buttonScanWindows);
            this.Controls.Add(this.textBoxWindows);
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.labelCheckResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Check DLL Sideloading";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkTextBox textBoxWindows;
        private DarkUI.Controls.DarkButton buttonScanWindows;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkTextBox textBoxApp;
        private DarkUI.Controls.DarkButton buttonOpenApp;
        private DarkUI.Controls.DarkButton buttonCheckApp;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DarkUI.Controls.DarkCheckBox checkFilter;
        private DarkUI.Controls.DarkButton buttonOpenFolder;
        private DarkUI.Controls.DarkLabel darkLabel3;
        private DarkUI.Controls.DarkButton buttonUpload;
        private System.Windows.Forms.ToolTip toolTip1;
        private DarkUI.Controls.DarkLabel labelCheckResult;
    }
}

